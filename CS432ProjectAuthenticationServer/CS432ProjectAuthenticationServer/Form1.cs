using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CS432ProjectAuthenticationServer;
using System.Security.Cryptography;
using System.IO;

namespace CS432ProjectAuthenticationServer
{
    public partial class Form1 : Form
    {
        static RichTextBox monitor;
        static TextBox portNumberTextBox;
        static Button serverStartButton;
        static Button browseServerPrivateKeyFileButton;
        static TextBox serverPublicPrivateKeyFilePathTextBox;
        static int PortNumber;

        static Socket possibleClient;

        static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> socketList = new List<Socket>();
        static List<string> nameList = new List<string>();
        static List<User> userList = new List<User>();

        Thread thrAccept;
        Thread thrMessage;

        static bool listening = false;
        static bool terminating = false;
        static bool accept = true;

        public static string serverDirectoryPath;
        private string SERVER_PUB_PRIVATE_KEY_PAIR_FILE_PATH;
        private string serverPublicPrivateKeyXmlString;
        //
        private MyCryptography MyCrypto = new MyCryptography();

        // For Random Number Generation
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            monitor = (RichTextBox)rtbMonitor;
            portNumberTextBox = (TextBox)tbPortNumber;
            serverStartButton = (Button)btnStartServer;
            browseServerPrivateKeyFileButton = (Button)btnBrowse;
            serverPublicPrivateKeyFilePathTextBox = (TextBox)tbServerPrivateKeyPath;

        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!serverPublicPrivateKeyFilePathTextBox.Text.Equals("") && File.Exists(serverPublicPrivateKeyFilePathTextBox.Text)) // The server choosed a file path
            {
                PortNumber = Convert.ToInt32(portNumberTextBox.Text);
                // Will create a server directory if it does not exists
                serverDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                serverDirectoryPath = serverDirectoryPath + "/Server";

                // Read the public_private key of server as a xml string
                SERVER_PUB_PRIVATE_KEY_PAIR_FILE_PATH = serverPublicPrivateKeyFilePathTextBox.Text;
                serverPublicPrivateKeyXmlString = File.ReadAllText(SERVER_PUB_PRIVATE_KEY_PAIR_FILE_PATH);

                if (!Directory.Exists(serverDirectoryPath))
                {
                    Directory.CreateDirectory(serverDirectoryPath);
                }

                server.Bind(new IPEndPoint(IPAddress.Any, PortNumber));
                monitor.AppendText("Started listening for incoming connections.\n");

                server.Listen(3);
                thrAccept = new Thread(new ThreadStart(Accept));
                thrAccept.Start();
            }
            else // The server did not choose a path for its private key file.
            {
                showNotificationBalloon("Please choose a private key file ");
            }
        }

        private void Accept()
        {
            while (accept)
            {
                try
                {
                    possibleClient = server.Accept();

                    Thread thrMessage;
                    //thrMessage = new Thread(new ThreadStart( ReceiveMessage ));                
                    thrMessage = new Thread(() => ReceiveMessage(possibleClient));
                    thrMessage.Start();

                }
                catch
                {
                    possibleClient.Close();
                    if (terminating)
                        accept = false;
                    else
                    //Console.Write("Listening socket has stopped working...\n");
                    { monitor.AppendText("Listening socket has stopped working...\n"); }
                }
            }
        }

        private void ReceiveMessage(Socket s)
        {
            bool connected = true;
            Socket n = s;

            byte[] namebuffer = new byte[3];
            int receivedNameCharCount = n.Receive(namebuffer);            
            String firstMessageFromClient = Encoding.Default.GetString(namebuffer);

            User possibleUser = new User("", n);
            HandleMessage(firstMessageFromClient, possibleUser);

            User currentUser = possibleUser;

            while (connected)
            {
                try
                {
                    Byte[] buffer = new byte[3];
                    int rec = currentUser.getSocket().Receive(buffer);

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                    HandleMessage(newmessage, currentUser);

                    Console.Write("Client: " + newmessage + "\r\n");
                }
                catch
                {
                    if (!terminating)
                        Console.Write("Client has disconnected...\n");
                    n.Close();
                    socketList.Remove(n);
                    connected = false;
                }
            }

        }


        public void HandleMessage(string message, User user)
        {

            switch (message)
            {
                case MessageNumber.AUTHENTICATION_REQUEST:
                    // Some body wanted to connect to the server
                    sendMessageNumber(MessageNumber.AUTHENTICATION_REQUEST_OK, user);
                    break;

                case MessageNumber.USERNAME:
                    // Somebody wants to send its username. Get its user name and
                    sendMessageNumber(MessageNumber.READY_TO_ACCEPT_USERNAME, user);
                    ReceiveUserName(user);
                    sendMessageNumber(MessageNumber.STARTED_CHALLENGE_RESPONSE, user);
                    break;

                case MessageNumber.STARTED_CHALLENGE_RESPONSE_OK:
                    // Generate a random number and send it as a string to the given user
                    byte[] randomNumber = new byte[128];
                    rngCsp.GetBytes(randomNumber);
                    string encodedRandomNumber = Encoding.Default.GetString(randomNumber);
                    user.setRandomNumberSent(randomNumber);
                    sendMessage(encodedRandomNumber, user);
                    break;

                case MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER:
                    sendMessageNumber(MessageNumber.WILL_SEND_SIGNED_RANDOM_NUMBER_OK, user);
                    
                    byte [] signedRandomNumber = receiveSignedRandomNumber(user);
                    byte[] randomNumberSent = user.getRandomNumberSentToUser();
                    string XmlString = File.ReadAllText(serverDirectoryPath + "/" + user.getUserName() + "_pub.txt" );
                    bool result =MyCrypto.verifyWithRSA(MyCrypto.generateHexStringFromByteArray( randomNumberSent),
                            2048, XmlString, signedRandomNumber );
                    user.setAuthenticated(result);
                    sendMessageNumber(MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE, user);
                    break;


                case MessageNumber.WILL_SEND_SIGNED_ANSWER_MESSAGE_OK:
                    // Here user is not yet authenticated
                    byte[] signedMessageNumber = null;
                    if(user.getAuthenticated() == true) // sign the MessageNumber.USER_AUTHENTICATED message
                    {
                        signedMessageNumber = MyCrypto.signWithRSA(MessageNumber.USER_AUTHENTICATED, 2048,
                                serverPublicPrivateKeyXmlString);
                    }
                    else // sign the MessageNumber.USER_REJECTED_TO_AUTHENTICATE message
                    {
                        signedMessageNumber = MyCrypto.signWithRSA(MessageNumber.USER_REJECTED_TO_AUTHENTICATE, 2048,
                                serverPublicPrivateKeyXmlString);
                    }
                    sendMessage(Encoding.Default.GetString(signedMessageNumber), user);
                    break;

                case MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY:
                    sendMessageNumber(MessageNumber.RECEIVED_RANDOM_NUMBER_SUCCESSFULLY_ACK, user);
                    break;



            }
        }


        //Sends numbers as strings(message numbers)
        static public void sendMessageNumber(String messageNumber, User u)
        {
            byte[] buffer = Encoding.Default.GetBytes(messageNumber.ToString());
            u.getSocket().Send(buffer);
        }

        //Sends string
        static public void sendMessage(String message, User u)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            u.getSocket().Send(buffer);
        }


        public void ReceiveUserName(User user)
        {
            byte[] namebuffer = new byte[64];
            int receivedNameCharCount = user.getSocket().Receive(namebuffer);
            String possibleUserName = Encoding.Default.GetString(namebuffer, 0, receivedNameCharCount);
            user.setUserName(possibleUserName);

            if (checkUserNameExists(user)) // User name exists
            {
                byte[] buffer = Encoding.Default.GetBytes(MessageNumber.USER_ALREADY_CONNECTED.ToString());
                monitor.AppendText("A client named " + user.getUserName() + " is rejected to connect \n");
                user.getSocket().Send(buffer);
                user.getSocket().Close();
            }
            else if (!checkUserNameExists(user)) // User name does not exist
            {
                ////byte[] buffer = Encoding.Default.GetBytes(MessageNumber.USER_ACCEPTED_TO_CONNECT.ToString());
                ////monitor.AppendText("A client named " + user.getUserName() + " is accepted \n");
                ////user.getSocket().Send(buffer);
                //userList.Add(user);
                //socketList.Add(user.getSocket());
                //nameList.Add(user.getUserName());

                //// Directory aç

                //String myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + user.getUserName();
                //System.IO.Directory.CreateDirectory(myDir);

                //String toDir = myDir + "\\" + "to";
                //String fromDir = myDir + "\\" + "from";
                //System.IO.Directory.CreateDirectory(toDir);
                //System.IO.Directory.CreateDirectory(fromDir);
            }
        }


        public bool checkUserNameExists(User user)
        {
            bool result = false;
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].getUserName().Equals(user.getUserName()))
                    result = true;
            }
            return result;
        }

        static string generateHexStringFromByteArray(byte[] input)
        {
            string hexString = BitConverter.ToString(input);
            return hexString.Replace("-", "");
        }

        // Returns the received 
        public byte [] receiveSignedRandomNumber(User user)
        {
            byte[] buffer = new byte[4096];
            int receivedCharCount = user.getSocket().Receive(buffer);
            byte[] result = new byte[receivedCharCount];
            Array.Copy(buffer, 0, result, 0, receivedCharCount);
            return result;  
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.Filter = "All Files (*.*)|*.*";
            Dlg.CheckFileExists = true;
            Dlg.Title = "Choose a File";
            Dlg.InitialDirectory = @"C:\";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                serverPublicPrivateKeyFilePathTextBox.Text = Dlg.FileName;
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
    }
}
