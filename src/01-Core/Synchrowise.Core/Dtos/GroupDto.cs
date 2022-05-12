using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Models;

namespace Synchrowise.Core.Dtos
{
    public class GroupDto
    {
        public Guid Guid { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int GroupMemberCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public GroupMemberDto GroupOwner { get; set; }
        public List<GroupMemberDto> GroupMember { get; set; }
        public List<GroupFileDto> GroupFiles { get; set; }
    }
}