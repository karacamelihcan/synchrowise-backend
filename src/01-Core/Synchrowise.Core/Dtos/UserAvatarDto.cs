using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Synchrowise.Core.Dtos
{
    public class UserAvatarDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Path { get; set; } = "Sources/Defaults/3af13787-a0f4-4ed0-888a-eb3a988c14e0.jpeg";
        public DateTime UploadDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid OwnerGuid { get; set; }
    }
}