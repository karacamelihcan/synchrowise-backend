using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Synchrowise.Services.MappingProfile
{
    public static class ObjectMapper
    {   
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() => 
        {
            var config =new MapperConfiguration(cfg => {
                cfg.AddProfile<DTOMapper>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper = lazy.Value;
    }
}