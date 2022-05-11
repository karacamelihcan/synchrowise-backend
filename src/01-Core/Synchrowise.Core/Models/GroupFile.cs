using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Synchrowise.Core.Models
{
    public class GroupFile
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Path { get; set; } 
        public string FolderPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid GroupGuid { get; set; }
        public bool isDeleted { get; set; } = false;

        [JsonIgnore]
        public virtual Group Group { get; set; }
    }
}