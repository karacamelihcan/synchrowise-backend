using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Newtonsoft.Json;
using Synchrowise.Enumeration.Enums;

namespace Synchrowise.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public Guid Guid { get; set; }
        [JsonIgnore]
        public string Firebase_uid { get; set; }
        [JsonIgnore]
        public string Firebase_id_token { get; set; }
        public string Username { get; set; }
        public string firebase_messaging_token { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public bool Email_verified { get; set; } = false;

        [JsonIgnore]
        public bool Is_New_user { get; set; } = true;
        [JsonIgnore]
        public string Signin_Method { get; set; }
        public DateTimeOffset Firebase_Creation_Time { get; set; }
        public DateTimeOffset Firebase_Last_Signin_Time { get; set; }
        [JsonIgnore]
        public int Term_Vision { get; set; }
        public EnumPremiumType PremiumType { get; set; } = 0;
        [JsonIgnore]
        public bool isHaveGroup { get; set; } = false;
        [JsonIgnore]
        public bool isDelete { get; set; } = false;
        [JsonIgnore]
        public Guid GroupId { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }

        public int AvatarID { get; set; }
        public virtual UserAvatar Avatar { get; set; }
        public NotificationSettings Notifications { get; set; }


    }
}