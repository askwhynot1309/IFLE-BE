using BusinessObjects.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Transaction.Response
{
    public class TransactionDetailsResponseModel
    {
        public string Id { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderCode { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string PackageName { get; set; } = null!;

        public string PackageCategory { get; set; } = null!;

        public UserInfoResponeModel User { get; set; } = null!;
    }
}
