using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class InteractiveFloor
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Length { get; set; }

        public bool IsPublic { get; set; }

        public string Status { get; set; } = null!;

        public string OrganizationId { get; set; } = null!;

        public string? DeviceId { get; set; }

        public virtual Device? Device { get; set; }

        public virtual Organization Organization { get; set; } = null!;

        public virtual ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();

        public virtual ICollection<PrivateFloorUser> PrivateFloorUsers { get; set; } = new List<PrivateFloorUser>();

        public virtual ICollection<GamePackageOrder> GamePackageOrders { get; set; } = new List<GamePackageOrder>();

    }
}
