using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Firebase_Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Term_Vision { get; set; }
        public bool isDelete { get; set; } = false;
    }
}