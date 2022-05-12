using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;

namespace Synchrowise.Services.MappingProfile
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<UserDto,User>().ReverseMap();
            CreateMap<GroupMemberDto,User>().ReverseMap();
            CreateMap<GroupDto,Group>().ReverseMap();
            CreateMap<UserAvatarDto,UserAvatar>().ReverseMap();
            CreateMap<GroupFile,GroupFileDto>().ReverseMap();
            CreateMap<NotificationSettingsDto,NotificationSettings>().ReverseMap();
        }
    }
}