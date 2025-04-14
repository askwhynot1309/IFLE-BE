using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class GamePackageOrder
    {
        public string Id { get; set; } = null!;

        public string? ActivationKey { get; set; } 

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? OrderCode { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public DateTime? ActivationDate { get; set; }

        public string Status { get; set; } = null!;

        public string FloorId { get; set; } = null!;

        public string GamePackageId { get; set; } = null!;

        public virtual InteractiveFloor InteractiveFloor { get; set; } = null!;

        public virtual GamePackage GamePackage { get; set; } = null!;
    }
}
