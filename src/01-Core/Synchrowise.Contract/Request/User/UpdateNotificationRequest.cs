using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.User
{
    public class UpdateNotificationRequest
    {
        public bool MessageNotification { get; set; }
        public bool GroupNotification { get; set; }
    }
}