using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Enumeration.Enums;

namespace Synchrowise.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Firebase_uid { get; set; }
        public string Firebase_id_token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Email_verified { get; set; } = false;
        public string Avatar { get; set; } = "src/images/default.jpg";
        public bool Is_New_user { get; set; } = true;
        public string Signin_Method { get; set; }
        public DateTimeOffset Firebase_Creation_Time { get; set; }
        public DateTimeOffset Firebase_Last_Signin_Time { get; set; }
        public int Term_Vision { get; set; }
        public EnumPremiumType PremiumType { get; set; } = 0;
        public bool isDelete { get; set; } = false;
    }
}