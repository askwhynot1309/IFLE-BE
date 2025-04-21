using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.GamePackage.Request
{
    public class GamePackageCreateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên gói game.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời hạn gói game (tháng).")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá gói game (VNĐ).")]
        [Range(2000, double.MaxValue, ErrorMessage = "Giá gói phải lớn hơn 2000đ.")]
        public decimal Price { get; set; }

        public List<string> GameIdList { get; set; } = new List<string>();
    }
}
