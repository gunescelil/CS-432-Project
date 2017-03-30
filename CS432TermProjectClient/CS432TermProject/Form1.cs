using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CS432TermProject
{
    public partial class Form1 : Form
    {
        // Server Related Variables
        static Socket client;
        string authenticationServerIP;
        int authenticationServerPort;
        bool connected;

        // Buttons
        Button connectButton;
        Button disconnectButton;

        // TextBox
        TextBox usernameTextBox, passwordTextBox, authenticationServerIPTextBox, authenticationServerPortTextBox;
        TextBox userKeyPairFilePathTextBox, authenticationServerPubKeyFilePathTextBox;
        public static string username;
        public static string password;

        // Client Monitor
        RichTextBox monitor;

        // Threads
        Thread thrMessage;

        private MyCryptography myCrypto;
        private byte[] decryptedXML2048BitKey = null;
        private byte[] receivedRandomNumber = null;

        // 
        public static string PATH_FOR_USER_KEY_PAIR;
        public static string PATH_FOR_AUTHENTICATION_SERVER_PUB_KEY;
        public static string PATH_FOR_FSERVER_PUB_KEY;

        string decryptedXml2048BitKeyString;
        string authenticationServerPublicKeyXmlString;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            connectButton = (Button)btnConnect;
            usernameTextBox = (TextBox)tbUserName;
            passwordTextBox = (TextBox)tbPassword;
            authenticationServerIPTextBox = (TextBox)tbAuthenticationServerIP;
            authenticationServerPortTextBox = (TextBox)tbAuthenticationServerPort;
            userKeyPairFilePathTextBox = (TextBox)tbUserKeyPairFile;
            authenticationServerPubKeyFilePathTextBox = (TextBox)tbAServerPubKeyFile;
            monitor = (RichTextBox) rtbMonitor;
            disconnectButton = (Button)btnDisconnect;
            disconnectButton.Enabled = false;

        }


        private void Connect_Click(object sender, EventArgs e)
        {
            
            myCrypto = new MyCryptography();
            if ( authenticationServerPubKeyFilePathTextBox.Text.Equals("") || userKeyPairFilePathTextBox.Text.Equals("") )
            { // The user did not choose the needed files for encryption. Make him choose.
                showNotificationBalloon("Please choose the key file paths first");
            }
            else
            { // User choosed the needed files

                connectButton.Enabled = false;
                disconnectButton.Enabled = true;
                authenticationServerIPTextBox.Enabled = false;
                authenticationServerPortTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                usernameTextBox.Enabled = false;
                userKeyPairFilePathTextBox.Enabled = false;
                authenticationServerPubKeyFilePathTextBox.Enabled = false;

                username = usernameTextBox.Text;
                Regex regex = new Regex("^([a-z][a-z0-9]+|[a-z]){1,25}$");

                password = passwordTextBox.Text;
                // Get the hash of the password
                byte[] hashedPasswordWithSHA256 = myCrypto.hashWithSHA256(password);

                byte[] KEY = new byte[16];
                // The first 16 bytes are the key for decryption of encrypted Private Key of User
                Array.Copy(hashedPasswordWithSHA256, 0, KEY, 0, 16);
                monitor.AppendText("The AES Key is:" + myCrypto.generateHexStringFromByteArray(KEY) +"\n");
                

                byte[] IV = new byte[16];
                // The last 16 bytes of the hashed password will be the Initialization Vector for decryption.
                Array.Copy(hashedPasswordWithSHA256, 16, IV, 0, 16);
                monitor.AppendText("The AES IV is:" + myCrypto.generateHexStringFromByteArray(IV) + "\n");

                // Get the paths from textboxes 
                PATH_FOR_USER_KEY_PAIR = userKeyPairFilePathTextBox.Text;
                PATH_FOR_AUTHENTICATION_SERVER_PUB_KEY = authenticationServerPubKeyFilePathTextBox.Text;

                string encryptedXML2048BitKey = readFromFile(PATH_FOR_USER_KEY_PAIR);
                monitor.AppendText("Encrypted XML 2048 Bit RSA Private_Public Key Pair:\n " + encryptedXML2048BitKey +"\n");
                byte[] byteArrayOfEncryptedXML2048BitKey = myCrypto.StringToByteArray(encryptedXML2048BitKey);
                monitor.AppendText("Hex string of encrypted XML 2048 Bit RSA Private_Public Key Pair: \n"
                        + myCrypto.generateHexStringFromByteArray(byteArrayOfEncryptedXML2048BitKey ) + "\n");
                string stringVersionOfEncryptedXML2048BitKey = Encoding.Default.GetString(byteArrayOfEncryptedXML2048BitKey);
                try {
                    decryptedXML2048BitKey = myCrypto.decryptWithAES128(stringVersionOfEncryptedXML2048BitKey, KEY, IV);
                    monitor.AppendText("Hex String of decrypted XML 2048 Bit Public_Private Key Pair: \n" 
                            + myCrypto.generateHexStringFromByteArray(decryptedXML2048BitKey) +"\n");
                    decryptedXml2048BitKeyString = Encoding.Default.GetString(decryptedXML2048BitKey);
                    //monitor.AppendText("String version of decrypted XML 2048 Bit Public_Private Key Pair: \n"
                           // + decryptedXml2048BitKeyString + "\n");

                }
                catch (Exception ex)
                {
                    connectButton.Enabled = true;
                    disconnectButton.Enabled = false;
                    authenticationServerIPTextBox.Enabled = true;
                    authenticationServerPortTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                    usernameTextBox.Enabled = true;
                    authenticationServerPubKeyFilePathTextBox.Enabled = true;
                    userKeyPairFilePathTextBox.Enabled = true;                 
                    throw (new Exception("Password is incorrect"));                   
                }
                

                // If the password given from user is incorrect then  decryptedXML2048BitKey will be null

                if(decryptedXML2048BitKey!=null) // Password is correct
                {
                    // Get the public key of server in a string
                    authenticationServerPublicKeyXmlString = File.ReadAllText(PATH_FOR_AUTHENTICATION_SERVER_PUB_KEY);

                    if (regex.IsMatch(username))
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        authenticationServerPort = Convert.ToInt32(authenticationServerPortTextBox.Text);
                        authenticationServerIP = authenticationServerIPTextBox.Text;

                        try
                        {
                            client.Connect(authenticationServerIP, authenticationServerPort);
                        }
                        catch
                        {
                            connectButton.Enabled = true;
                            disconnectButton.Enabled = false;
                            authenticationServerIPTextBox.Enabled = true;
                            authenticationServerPortTextBox.Enabled = true;
                            passwordTextBox.Enabled = true;
                            usernameTextBox.Enabled = true;
                            authenticationServerPubKeyFilePathTextBox.Enabled = true;
                            userKeyPairFilePathTextBox.Enabled = true;
                            throw (new Exception("IP or Port numbers may be wrong"));                            
                        }
                        // Changing UI
                        

                        SendMessageNumber(MessageNumber.AUTHENTICATION_REQUEST);

                        thrMessage = new Thread(new ThreadStart(receiveMessage));
                        thrMessage.Start();
                    }
                }
                else
                {
                    connectButton.Enabled = true;
                    disconnectButton.Enabled = false;
                    authenticationServerIPTextBox.Enabled = true;
                    authenticationServerPortTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                    passwordTextBox.Clear();
                    usernameTextBox.Enabled = true;
                    authenticationServerPubKeyFilePathTextBox.Enabled = true;
                    userKeyPairFilePathTextBox.Enabled = true;
                }
            }            
        }

        // Sends message numbers as strings
        static void SendMessageNumber(string number)
        {
            byte[] buffer = Encoding.Default.GetBytes(number.ToString());
            client.Send(buffer);
        }

        // Sends strings
        static void SendMessage(string text)
        {
            byte[] buffer = Encoding.Default.GetBytes(text);
            client.Send(buffer);
        }

        private void btnBrowseAServerPublicKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.Filter = "All Files (*.*)|*.*";
            Dlg.CheckFileExists = true;
            Dlg.Title = "Choose a File";
            Dlg.InitialDirectory = @"C:\";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                authenticationServerPubKeyFilePathTextBox.Text = Dlg.FileName;
            }
        }

        private void btnBrowseUserKeyPairFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.Filter = "All Files (*.*)|*.*";
            Dlg.CheckFileExists = true;
            Dlg.Title = "Choose a File";
            Dlg.InitialDirectory = @"C:\";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                userKeyPairFilePathTextBox.Text = Dlg.FileName;
            }
        }

        private void receiveMessage()
        {
            connected = true;
            while (connected)
            {
                HandleMessage();
            }
        }

        public void HandleMessage()
        {
            byte[] response = new byte[3];
            client.Receive(response);

            string message = Encoding.Default.GetString(response);           
            switch (message)
            {
                case MessageNumber.AUTHENTICATION_REQUEST_OK:
                    SendMessageNumber(MessageNumber.USERNAME);
                    break;

                case MessageNumber.READY_TO_ACCEPT_USERNAME:
                    SendMessage(username);                    
                    break;

                case MessageNumber.STARTED_CHALLENGE_RESPONSE:
                    SendMessageNumber(MessageNumber.STARTED_CHALLENGE_RESPONSE_OK);
                    monitor.AppendText("Server started challenge-response protocol.\n");
                    receivedRandomNumber = receiveRandomNumber();
                    monitor.AppendText("Random Number from Server is: \n" 
                             + myCrypto.generateHexStringFromByteArray(receivedRandomNumber) +"\n" );
                    SendMessageNumber(MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY);
                    break;

                case MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY_ACK:
                    SendMessageNumber(MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER);
                    break;

                case MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER_OK:
                    monitor.AppendText("Will send signed random number\n");
                    byte[] signedRandomNumber = myCrypto.signWithRSA(myCrypto.generateHexStringFromByteArray(receivedRandomNumber), 
                        2048, decryptedXml2048BitKeyString );
                    monitor.AppendText("signed random number is \n" 
                            + myCrypto.generateHexStringFromByteArray(signedRandomNumber) +"\n" );
                    SendMessage(Encoding.Default.GetString(signedRandomNumber));
                    break;

                case MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE:
                    SendMessageNumber(MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE_OK);
                    monitor.AppendText("Server will send signed message \n");
                    byte [] signedMessageNumber = receiveSignedMessageNumber();
                    monitor.AppendText("Signed message number is: \n" 
                            + myCrypto.generateHexStringFromByteArray(signedMessageNumber) + "\n");
                    bool authenticatonPositiveResult = myCrypto.verifyWithRSA(MessageNumber.USER_AUTHENTICATED ,2048, 
                            authenticationServerPublicKeyXmlString, signedMessageNumber);

                    bool authenticationNegativeResult = myCrypto.verifyWithRSA(MessageNumber.USER_REJECTED_TO_AUTHENTICATE, 2048,
                            authenticationServerPublicKeyXmlString, signedMessageNumber);

                    // if both of them are false. Than this message is not from the server.
                    if(authenticatonPositiveResult)
                    { // So the server authenticated the client. Client knows this message comes from the server he/she spokes.
                        monitor.AppendText("User is authenticated\n");
                    }
                    else if(authenticationNegativeResult)
                    { // The server did not give permission to access. Client knows this message comes from the server he/she spokes.
                        monitor.AppendText("Access denied\n");
                        if (client.Connected)
                        {
                            connected = false;
                            client.Disconnect(false); }
                        onAccessDenied();
                    }
                    else
                    {
                        monitor.AppendText("Received an unexpected message\n");
                    }

                    break;

                case MessageNumber.USER_ALREADY_CONNECTED:
                    monitor.AppendText("User is already connected\n");
                    connectButton.Enabled = true;
                    authenticationServerIPTextBox.Enabled = true;
                    authenticationServerPortTextBox.Enabled = true;
                    passwordTextBox.Enabled = true;
                    usernameTextBox.Enabled = true;
                    authenticationServerPubKeyFilePathTextBox.Enabled = true;
                    userKeyPairFilePathTextBox.Enabled = true;
                    break;

                case MessageNumber.USER_AUTHENTICATED:
                    monitor.AppendText("User is authenticated\n");
                    break;

                case MessageNumber.USER_REJECTED_TO_AUTHENTICATE:
                    monitor.AppendText("User is not authenticated\n");
                    break;

                case MessageNumber.WILL_DISCONNECT_OK:
                    client.Disconnect(true); // As we get new Socket in each connection. I said it wont be used again.                    
                    break;
                case MessageNumber.SERVER_WILL_CLOSE:
                    // Server will close itself down. So disconnect from the server
                    onAuthenticationServerClosedItself();
                    break;

    }
        }

        public void onAccessDenied()
        {
            PATH_FOR_USER_KEY_PAIR = null;
            PATH_FOR_FSERVER_PUB_KEY = null;
            PATH_FOR_FSERVER_PUB_KEY = null;
            authenticationServerPublicKeyXmlString = null;
            decryptedXML2048BitKey = null;
            decryptedXml2048BitKeyString = null;
            receivedRandomNumber = null;

            // Enable all the buttons again.
            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            authenticationServerIPTextBox.Enabled = true;
            authenticationServerPortTextBox.Enabled = true;
            passwordTextBox.Enabled = true;
            usernameTextBox.Enabled = true;
            authenticationServerPubKeyFilePathTextBox.Enabled = true;
            userKeyPairFilePathTextBox.Enabled = true;
        }

        public void onAuthenticationServerClosedItself()
        {
            connected = false;
            client.Disconnect(false);
            PATH_FOR_USER_KEY_PAIR = null;
            PATH_FOR_FSERVER_PUB_KEY = null;
            PATH_FOR_FSERVER_PUB_KEY = null;
            authenticationServerPublicKeyXmlString = null;
            decryptedXML2048BitKey = null;
            decryptedXml2048BitKeyString = null;
            receivedRandomNumber = null;

            // Enable all the buttons again.
            connectButton.Enabled = true;
            disconnectButton.Enabled = false;
            authenticationServerIPTextBox.Enabled = true;
            authenticationServerPortTextBox.Enabled = true;
            passwordTextBox.Enabled = true;
            usernameTextBox.Enabled = true;
            authenticationServerPubKeyFilePathTextBox.Enabled = true;
            userKeyPairFilePathTextBox.Enabled = true;

            monitor.AppendText("Server closed itself");
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            connected = false;
            if (client.Connected) // the client is connected. So client will be disconnected
            {
                // Send disconnection message to server.
                SendMessageNumber(MessageNumber.WILL_DISCONNECT);             
                // Clear all the private information on the memory                      
                PATH_FOR_USER_KEY_PAIR = null;
                PATH_FOR_FSERVER_PUB_KEY = null;
                PATH_FOR_FSERVER_PUB_KEY = null;
                authenticationServerPublicKeyXmlString = null;
                decryptedXML2048BitKey = null;
                decryptedXml2048BitKeyString = null;
                username = null;
                password = null;
                receivedRandomNumber = null;
                userKeyPairFilePathTextBox.Clear();
                authenticationServerPubKeyFilePathTextBox.Clear();
                authenticationServerIPTextBox.Clear();
                authenticationServerPortTextBox.Clear();

                // Enable all the buttons again.
                connectButton.Enabled = true;
                disconnectButton.Enabled = false;
                authenticationServerIPTextBox.Enabled = true;
                authenticationServerPortTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                passwordTextBox.Clear();
                usernameTextBox.Enabled = true;
                usernameTextBox.Clear();
                authenticationServerPubKeyFilePathTextBox.Enabled = true;
                userKeyPairFilePathTextBox.Enabled = true;
                // Clear the monitor maybe?

                monitor.Clear();

                
            }
            else
            {
                monitor.AppendText("Client is already not connected\n ");
            }
        }


        // This function is called when user want to close the program by clicking on cross sign of the form.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null) { 
                if (client.Connected)
                {
                    connected = false;
                    SendMessageNumber(MessageNumber.WILL_DISCONNECT);
                    client.Disconnect(false);
                }
            }
        }


        public void showNotificationBalloon(string message)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                // optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                // optional - BalloonTipTitle = "My Title",
                BalloonTipText = message,
            };
            // Display for 3 seconds.
            notification.ShowBalloonTip(3);
            notification.Dispose();
        }


        public string readFromFile(string path)
        {
            if (File.Exists(path))
            { return File.ReadAllText(path); }
            else return null;
        }

        public byte[] receiveRandomNumber()
        {
            byte[] buffer = new byte[512];
            int receivedCharacterCount = client.Receive(buffer);
            byte[] result = new byte[receivedCharacterCount];
            Array.Copy(buffer, 0, result, 0, receivedCharacterCount);
            return result;
        }

        public byte[] receiveSignedMessageNumber()
        {
            // Not sure how much character will come from the server. May need to implement other ways of sending that 
            byte[] buffer = new byte[512];
            int receivedCharCount = client.Receive(buffer);
            byte[] result = new byte[receivedCharCount];
            Array.Copy(buffer, 0, result, 0, receivedCharCount);
            return result;
        }







    }
}
