using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class UserPackage
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int UserLimit { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<UserPackageOrder> UserPackageOrders { get; set; } = new List<UserPackageOrder>();

    }
}
