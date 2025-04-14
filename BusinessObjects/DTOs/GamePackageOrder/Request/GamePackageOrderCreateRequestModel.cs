using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.GamePackageOrder.Request
{
    public class GamePackageOrderCreateRequestModel
    {
        [Required(ErrorMessage = "Thiếu return url")]
        public string ReturnUrl { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu cancel url")]
        public string CancelUrl { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu phương thức thanh toán")]
        public string PaymentMethod { get; set; } = null!;

        [Required(ErrorMessage = "Thiếu game package id")]
        public string GamePackageId { get; set; } = null!;
    }
}
