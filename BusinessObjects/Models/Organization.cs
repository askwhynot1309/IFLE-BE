using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Organization
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int UserLimit { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();

        public virtual ICollection<UserPackageOrder> UserPackageOrders { get; set; } = new List<UserPackageOrder>();

        public virtual ICollection<InteractiveFloor> InteractiveFloors { get; set; } = new List<InteractiveFloor>();
    }
}
