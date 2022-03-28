using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;

namespace Synchrowise.Services.MappingProfile
{
    public static class CustomMapping
    {
        public static GroupDto MappingGroup(Group group){
            var groupDto = new GroupDto(){
                Id = group.Id,
                Guid = group.Guid,
                GroupName = group.GroupName,
                GroupMemberCount = group.Users.Count,
                CreatedDate = group.CreatedDate,
                GroupOwner = group.Owner,
                GroupMember = group.Users
            };
            return groupDto;
        }
    }
}