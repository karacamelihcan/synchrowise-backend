using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Models
{
    public class NotificationSettings
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public bool MessageNotification { get; set; }
        public bool GroupNotification { get; set; }
        public int  UserId { get; set; }
        public User Owner { get; set; }

    }
}