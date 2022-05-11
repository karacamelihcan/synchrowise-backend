using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Models
{
    public class Group
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string GroupName { get; set; }
        public int GroupMemberCount { get; set; } = 1;
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid OwnerGuid { get; set; }
        public User Owner { get; set; }
        public List<User> Users { get; set; }
        public List<GroupFile> GroupFiles { get; set; }

    }
}