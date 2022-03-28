using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.Group
{
    public class CreateGroupRequest
    {
        public string GroupName { get; set; }
        public Guid OwnerID { get; set; }
    }
}