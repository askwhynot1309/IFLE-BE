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
        [Range(1, 12, ErrorMessage = "Chỉ được nhập 1-12 tháng.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá gói game (VNĐ).")]
        [Range(100000, 1000000, ErrorMessage = "Giá gói phải lớn hơn 100.000đ và bé hơn 1.000.000đ.")]
        public decimal Price { get; set; }

        public List<string> GameIdList { get; set; } = new List<string>();
    }
}
