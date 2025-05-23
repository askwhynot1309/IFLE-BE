﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.InteractiveFloor.Request
{
    public class FloorCreateUpdateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sàn.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chiều cao chỗ chứa sàn (m).")]
        [Range(2, 10, ErrorMessage = "Kích thước phần đất chứa sàn phải lớn hơn 2 và bé hơn 10.")]
        public float Height { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập độ rộng chỗ chứa sàn (m).")]
        [Range(2, 10, ErrorMessage = "Kích thước phần đất chứa sàn phải lớn hơn 2 và bé hơn 10.")]
        public float Width { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập độ dài chỗ chứa sàn (m).")]
        [Range(2, 10, ErrorMessage = "Kích thước phần đất chứa sàn phải lớn hơn 2 và bé hơn 10.")]
        public float Length { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn xem sàn có bị giới hạn người dùng không.")]
        public bool IsPublic { get; set; }
    }
}
