using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Device.Request
{
    public class DeviceCreateUpdateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên thiết bị.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập mô tả thiết bị.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng tải ứng dụng máy tính để lấy mã Uri của thiết bị.")]
        public string Uri { get; set; } = null!;

        public string? DeviceCategoryId { get; set; }
    }
}
