using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.Group
{
    public class RemoveGroupMemberRequest
    {
        public Guid GroupID { get; set; }
        public Guid OwnerId { get; set; }
        public Guid MemberID { get; set; }
    }
}