using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.UserPackageOrder.Request
{
    public class UserPackageOrderCreateRequestModel
    {
        [Required(ErrorMessage = "Thiếu return url")]
        public string ReturnUrl { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu cancel url")]
        public string CancelUrl { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu phương thức thanh toán")]
        public string PaymentMethod { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu user package id")]
        public string UserPackageId { get; set; } = null!;
    }
}
