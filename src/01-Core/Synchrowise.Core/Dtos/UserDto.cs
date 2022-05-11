using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Enumeration.Enums;

namespace Synchrowise.Core.Dtos
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserAvatarDto Avatar { get; set; }
        public EnumPremiumType PremiumType { get; set; }
        public bool MessageNotification { get; set; } 
        public bool GroupNotification { get; set; }
    }
}