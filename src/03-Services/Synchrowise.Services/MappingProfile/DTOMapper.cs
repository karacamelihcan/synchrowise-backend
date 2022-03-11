using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Synchrowise.Core.DTOs;
using Synchrowise.Core.Models;

namespace Synchrowise.Services.MappingProfile
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<UserDto,User>().ReverseMap();
        }
    }
}