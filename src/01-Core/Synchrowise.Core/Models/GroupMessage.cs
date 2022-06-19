using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synchrowise.Core.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Message { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public virtual User Sender { get; set; }
        public virtual Group Group { get; set; }

    }
}