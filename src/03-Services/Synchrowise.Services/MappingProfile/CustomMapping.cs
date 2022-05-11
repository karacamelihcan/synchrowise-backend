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
                Guid = group.Guid,
                GroupName = group.GroupName,
                GroupMemberCount = group.Users.Count,
                CreatedDate = group.CreatedDate
            };
            var groupOwner = ObjectMapper.Mapper.Map<UserDto>(group.Owner);
            var MemberList = new List<UserDto>();

            foreach (var user in group.Users.ToList())
            {
                MemberList.Add(ObjectMapper.Mapper.Map<UserDto>(user));
            }
            groupDto.GroupOwner = groupOwner;
            groupDto.GroupMember = MemberList;

            var files = new List<GroupFileDto>();

            foreach (var file in group.GroupFiles.ToList())
            {
                files.Add(ObjectMapper.Mapper.Map<GroupFileDto>(file));
            }
            groupDto.GroupFiles = files;

            return groupDto;
        }
    }
}