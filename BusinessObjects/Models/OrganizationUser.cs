using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class OrganizationUser
    {
        public string Id { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string OrganizationId { get; set; } = null!;

        public string Privilege { get; set; } = null!;

        public DateTime JoinedAt { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual Organization Organization { get; set; } = null!;
    }
}
