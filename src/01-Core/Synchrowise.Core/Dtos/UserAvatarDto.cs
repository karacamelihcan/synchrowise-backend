using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Synchrowise.Core.Dtos
{
    public class UserAvatarDto
    {
        public Guid Guid { get; set; }
        public string Url { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid OwnerGuid { get; set; }
    }
}