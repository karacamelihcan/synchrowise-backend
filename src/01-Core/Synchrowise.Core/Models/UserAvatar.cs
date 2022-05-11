using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Synchrowise.Core.Models
{
    public class UserAvatar
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Url { get; set; }
        public string FolderPath { get; set; } = "Sources/Defaults/3af13787-a0f4-4ed0-888a-eb3a988c14e0.jpeg";
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerID { get; set; }
        public Guid OwnerGuid { get; set; }
        public bool isDeleted { get; set; }

        [JsonIgnore]
        public virtual User Owner { get; set; }

    }
}