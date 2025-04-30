using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.UserPackage.Request
{
    public class UserPackageCreateUpdateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên gói nâng cấp người dùng.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mô tả gói nâng cấp giới hạn thành viên.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập giá của gói.")]
        [Range(100000, 1000000, ErrorMessage = "Giá gói phải lớn hơn 100.000đ và bé hơn 1.000.000đ.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới hạn số lượng người dùng.")]
        [Range(1, double.MaxValue, ErrorMessage = "Vui lòng nhập giá trị lớn hơn 1.")]
        public int UserLimit { get; set; }
    }
}
