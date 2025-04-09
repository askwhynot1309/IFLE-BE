using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class GamePackage
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<GamePackageOrder> GamePackageOrders { get; set; } = new List<GamePackageOrder>();

        public virtual ICollection<GamePackageRelation> GamePackageRelations { get; set; } = new List<GamePackageRelation>();

    }
}
