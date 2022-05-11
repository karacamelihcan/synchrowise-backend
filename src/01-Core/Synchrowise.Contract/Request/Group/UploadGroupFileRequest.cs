using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Synchrowise.Contract.Request.Group
{
    public class UploadGroupFileRequest
    {
        public Guid GroupGuid { get; set; }
        public Guid OwnerGuid { get; set; }
        public IFormFile File { get; set; }
    }
}