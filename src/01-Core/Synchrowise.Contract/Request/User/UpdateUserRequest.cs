using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Enumeration.Enums;

namespace Synchrowise.Contract.Request.User
{
    public class UpdateUserRequest
    {
        public string Firebase_id_token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Email_verified { get; set; }
        public long Firebase_Last_Signin_Time { get; set; }
        public string firebase_messaging_token { get; set; }
        public EnumPremiumType PremiumType { get; set; }
    }
}