using System;
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

        [Required(ErrorMessage = "Vui lòng nhập chiều cao của phòng (m).")]
        [Range(0, 100, ErrorMessage = "Chiều cao phòng phải lớn hơn 0 và bé hơn 100.")]
        public float Height { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập độ rộng của sàn (m).")]
        [Range(0, 100, ErrorMessage = "Độ rộng phòng phải lớn hơn 0 và bé hơn 100.")]
        public float Width { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập độ dài của sàn (m).")]
        [Range(0, 100, ErrorMessage = "Độ dài phòng phải lớn hơn 0 và bé hơn 100.")]
        public float Length { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn xem sàn có bị giới hạn người dùng không.")]
        public bool IsPublic { get; set; }
    }
}
