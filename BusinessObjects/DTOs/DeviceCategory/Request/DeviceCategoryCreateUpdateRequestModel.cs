using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.DeviceCategory.Request
{
    public class DeviceCategoryCreateUpdateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên loại thiết bị.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập khoảng cách đọc chiều sâu tối thiểu của thiết bị.")]
        [Range(0.1, 20, ErrorMessage = "Giá trị phải nằm trong khoảng 0.1-20.")]
        public float MinDetectionRange { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập khoảng cách đọc chiều sâu tối đa của thiết bị.")]
        [Range(0.1, 20, ErrorMessage = "Giá trị phải nằm trong khoảng 0.1-20.")]
        public float MaxDetectionRange { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập góc quét ngang của thiết bị.")]
        [Range(30, 80, ErrorMessage = "Giá trị phải nằm trong khoảng 30-80.")]
        public float HFov { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập góc quét dọc của thiết bị.")]
        [Range(30, 80, ErrorMessage = "Giá trị phải nằm trong khoảng 30-80.")]
        public float VFov { get; set; }

        public string? DeviceInfoUrl { get; set; }

        public string? UpdateStatus { get; set; }
    }
}
