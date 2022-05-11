using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.Group
{
    public class UpdateGroupInfoRequest
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public Guid GroupId { get; set; }
        public Guid OwnerId { get; set; }
    }
}