using BusinessObjects.DTOs.GamePackage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.GamePackageOrder.Response
{
    public class GamePackageWithAllOrderResponseModel
    {
        public GamePackageDetailsResponseModel? GamePackageInfo { get; set; }

        public List<OrderDetailsInfo>? OrderList { get; set; }
    }

    public class OrderDetailsInfo
    {
        public string Id { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? OrderCode { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public bool? IsActivated { get; set; }

        public string Status { get; set; } = null!;
    }

}
