using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Dtos
{
    public class GroupFileDto
    {
        public Guid Guid { get; set; }
        public string Path { get; set; } 
        public DateTime CreatedDate { get; set; }
        
    }
}