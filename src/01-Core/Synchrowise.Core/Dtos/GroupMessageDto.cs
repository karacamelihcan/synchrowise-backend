using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Dtos
{
    public class GroupMessageDto
    {
        public Guid Guid { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; } 
        public UserDto Sender { get; set; }
    }
}