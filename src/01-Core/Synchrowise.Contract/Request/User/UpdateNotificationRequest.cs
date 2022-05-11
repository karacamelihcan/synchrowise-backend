using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.User
{
    public class UpdateNotificationRequest
    {
        public Guid UserGuid { get; set; }
        public bool MessageNotification { get; set; }
        public bool GroupNotification { get; set; }
    }
}