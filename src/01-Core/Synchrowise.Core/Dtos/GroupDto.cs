using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Models;

namespace Synchrowise.Core.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string GroupName { get; set; }
        public int GroupMemberCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public User GroupOwner { get; set; }
        public List<User> GroupMember { get; set; }
    }
}