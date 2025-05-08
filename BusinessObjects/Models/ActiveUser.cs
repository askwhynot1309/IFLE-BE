using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class ActiveUser
    {
        public string Id { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public DateTime LoginTime { get; set; }

        public bool IsActive { get; set; }

        public virtual User User { get; set; } = null!;
    }
} 