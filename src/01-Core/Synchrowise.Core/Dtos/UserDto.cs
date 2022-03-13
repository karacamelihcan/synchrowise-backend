using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Enumeration.Enums;

namespace Synchrowise.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public EnumPremiumType PremiumType { get; set; }
    }
}