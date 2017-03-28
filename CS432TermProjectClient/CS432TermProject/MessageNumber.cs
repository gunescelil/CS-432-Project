using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS432TermProject
{

    // This class is for sending action information to server. It will keep the message/action types
    class MessageNumber
    {                         
        public const string USERNAME = "101";
        public const string READY_TO_ACCEPT_USERNAME = "102";
        public const string USER_ALREADY_CONNECTED = "103";
        public const string USER_ACCEPTED_TO_CONNECT = "104";

        public const string AUTHENTICATION_REQUEST = "105";
        public const string AUTHENTICATION_REQUEST_OK = "106";

        public const string STARTED_CHALLENGE_RESPONSE = "107";
        public const string STARTED_CHALLENGE_RESPONSE_OK = "108";

        public const string WILL_SEND_SIGNED_RANDOM_NUMBER = "109";
        public const string WILL_SEND_SIGNED_RANDOM_NUMBER_OK = "110";

        public const string USER_AUTHENTICATED = "111";
        public const string USER_REJECTED_TO_AUTHENTICATE = "112";

        public const string RECEIVED_RANDOM_NUMBER_SUCCESSFULLY = "113";
        public const string RECEIVED_RANDOM_NUMBER_SUCCESSFULLY_ACK = "114";

    }
}
