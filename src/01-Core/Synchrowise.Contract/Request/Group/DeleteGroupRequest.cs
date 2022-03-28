using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Contract.Request.Group
{
    public class DeleteGroupRequest
    {
        public Guid GroupId { get; set; }
        public Guid UserID { get; set; }
    }
}