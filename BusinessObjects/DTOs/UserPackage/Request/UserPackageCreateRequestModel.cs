using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.UserPackage.Request
{
    public class UserPackageCreateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mô tả gói nâng cấp giới hạn thành viên.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        [Range(2000, double.MaxValue, ErrorMessage = "Giá gói phải lớn hơn 2000đ.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới hạn số lượng người dùng.")]
        public int UserLimit { get; set; }
    }
}
