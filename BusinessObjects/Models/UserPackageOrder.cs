using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class UserPackageOrder
    {
        public string Id { get; set; } = null!;

        public decimal Price { get; set; }  

        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string UserPackageId { get; set; } = null!;

        public string OrganizationId { get; set; } = null!;

        public virtual Organization Organization { get; set; } = null!;

        public virtual UserPackage UserPackage { get; set; } = null!;
    }
}
