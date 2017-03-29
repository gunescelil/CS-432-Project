using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CS432ProjectAuthenticationServer
{
    public class User
    {
        String username;
        Socket socket;
        int NumberOfPacketsToReceive;
        private byte[] randomNumberSent;
        private string PublicKeyXmlString;
        bool authenticated;


        public User(String u, Socket s)
        {
            username = u;
            socket = s;
        }

        public void setUserName(String s)
        {
            username = s;
        }

        public string getUserName()
        {
            return username;
        }

        public Socket getSocket()
        {
            return socket;
        }

        public void setPacketNumber(int i)
        {
            NumberOfPacketsToReceive = i;
        }

        public int getPacketNumber()
        {
            return NumberOfPacketsToReceive;
        }

        public void setRandomNumberSent(byte[] number)
        {
            randomNumberSent = number;
        }

        public byte [] getRandomNumberSentToUser()
        {
            return randomNumberSent;
        }

        public void setPublicKeyXmlString(string xmlString)
        {
            PublicKeyXmlString = xmlString;
        }

        public string getPublicKeyXmlString()
        {
            return PublicKeyXmlString;
        }

        public void setAuthenticated(bool value)
        {
            authenticated = value;
        }

        public bool getAuthenticated()
        {
            return authenticated;
        }

    }
}
