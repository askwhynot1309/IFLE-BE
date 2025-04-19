using BusinessObjects.DTOs.UserPackage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.UserPackageOrder.Response
{
    public class UserPackageOrderListResponseModel
    {
        public string Id { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderCode { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string UserPackageId { get; set; } = null!;

        public UserPackageListResponseModel? UserPackageInfo { get; set; } 
    }
}
