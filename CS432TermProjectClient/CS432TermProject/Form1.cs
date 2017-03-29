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

        // TextBox
        TextBox usernameTextBox, passwordTextBox, authenticationServerIPTextBox, authenticationServerPortTextBox;
        TextBox userKeyPairFilePathTextBox, authenticationServerPubKeyFilePathTextBox;
        public static string username;
        public static string password;

        // Client Monitor
        RichTextBox Monitor;

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
            InitializeComponent();
            connectButton = (Button)btnConnect;
            usernameTextBox = (TextBox)tbUserName;
            passwordTextBox = (TextBox)tbPassword;
            authenticationServerIPTextBox = (TextBox)tbAuthenticationServerIP;
            authenticationServerPortTextBox = (TextBox)tbAuthenticationServerPort;
            userKeyPairFilePathTextBox = (TextBox)tbUserKeyPairFile;
            authenticationServerPubKeyFilePathTextBox = (TextBox)tbAServerPubKeyFile;
            Monitor = (RichTextBox) rtbMonitor;

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
                username = usernameTextBox.Text;
                Regex regex = new Regex("^([a-z][a-z0-9]+|[a-z]){1,25}$");

                password = passwordTextBox.Text;
                // Get the hash of the password
                byte[] hashedPasswordWithSHA256 = myCrypto.hashWithSHA256(password);

                byte[] KEY = new byte[16];
                // The first 16 bytes are the key for decryption of encrypted Private Key of User
                Array.Copy(hashedPasswordWithSHA256, 0, KEY, 0, 16);
                

                byte[] IV = new byte[16];
                // The last 16 bytes of the hashed password will be the Initialization Vector for decryption.
                Array.Copy(hashedPasswordWithSHA256, 16, IV, 0, 16);
                

                // Get the paths from textboxes 
                PATH_FOR_USER_KEY_PAIR = userKeyPairFilePathTextBox.Text;
                PATH_FOR_AUTHENTICATION_SERVER_PUB_KEY = authenticationServerPubKeyFilePathTextBox.Text;

                string encryptedXML2048BitKey = readFromFile(PATH_FOR_USER_KEY_PAIR);
                byte[] byteArrayOfEncryptedXML2048BitKey = myCrypto.StringToByteArray(encryptedXML2048BitKey);
                string stringVersionOfEncryptedXML2048BitKey = Encoding.Default.GetString(byteArrayOfEncryptedXML2048BitKey);             
                try
                {
                    // TODO: the xml is not correctly get from the 
                    decryptedXML2048BitKey = myCrypto.decryptWithAES128(stringVersionOfEncryptedXML2048BitKey, KEY, IV);
                    decryptedXml2048BitKeyString = Encoding.Default.GetString(decryptedXML2048BitKey);
                }
                catch (Exception exc )
                {
                    rtbMonitor.AppendText("Password entered is incorrect");
                    // TODO: Handle the exception and return the user to the password entering phase again
                }

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
                        rtbMonitor.AppendText("Something went wrong while connecting.\n");
                    }
                    // Changing UI
                    connectButton.Enabled = false;

                    SendMessageNumber(MessageNumber.AUTHENTICATION_REQUEST);

                    thrMessage = new Thread(new ThreadStart(receiveMessage));
                    thrMessage.Start();
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
                    receivedRandomNumber = receiveRandomNumber();
                    SendMessageNumber(MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY);
                    break;

                case MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY_ACK:
                    SendMessageNumber(MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER);
                    break;

                case MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER_OK:
                    byte[] signedRandomNumber = myCrypto.signWithRSA(myCrypto.generateHexStringFromByteArray(receivedRandomNumber), 
                        2048, decryptedXml2048BitKeyString );
                    SendMessage(Encoding.Default.GetString(signedRandomNumber));
                    break;

                case MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE:
                    SendMessageNumber(MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE_OK);
                    byte [] signedMessageNumber = receiveSignedMessageNumber();
                    bool authenticatonPositiveResult = myCrypto.verifyWithRSA(MessageNumber.USER_AUTHENTICATED ,2048, 
                            authenticationServerPublicKeyXmlString, signedMessageNumber);

                    bool authenticationNegativeResult = myCrypto.verifyWithRSA(MessageNumber.USER_REJECTED_TO_AUTHENTICATE, 2048,
                            authenticationServerPublicKeyXmlString, signedMessageNumber);

                    // if both of them are false. Than this message is not from the server.
                    if(authenticatonPositiveResult)
                    { // So the server authenticated the client. Client knows this message comes from the server he/she spokes.
                        Monitor.AppendText("Server gave access to the client");
                    }
                    else if(authenticationNegativeResult)
                    { // The server did not give permission to access. Client knows this message comes from the server he/she spokes.
                        Monitor.AppendText("Access denied");
                    }
                    else
                    {
                        Monitor.AppendText("Received an unexpected message");
                    }

                    break;
                case MessageNumber.USER_ACCEPTED_TO_CONNECT:
                    Monitor.AppendText("User is connected");
                    break;

                case MessageNumber.USER_ALREADY_CONNECTED:
                    Monitor.AppendText("User is already connected");
                    break;

                case MessageNumber.USER_AUTHENTICATED:
                    Monitor.AppendText("User is authenticated");
                    break;

                case MessageNumber.USER_REJECTED_TO_AUTHENTICATE:
                    Monitor.AppendText("User is not authenticated");
                    break;
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
