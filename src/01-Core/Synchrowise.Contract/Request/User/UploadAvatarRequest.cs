using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Synchrowise.Contract.Request.User
{
    public class UploadAvatarRequest
    {
        public Guid ownerId { get; set; }
        public IFormFile file { get; set; }
    }
}