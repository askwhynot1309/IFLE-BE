using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.SetUpGuide.Request
{
    public class SetUpGuideRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập độ cao bạn đặt camera.")]
        [Range(2, 3, ErrorMessage = "Chỉ nên đặt camera ở độ cao 2-3m so với mặt đất.")]
        public float CameraHeight { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập góc nghiêng bạn muốn.")]
        [Range(30, 45, ErrorMessage = "Vui lòng đặt camera chỉ nghiêng 1 góc 30-45 độ so với mặt phẳng ngang.")]
        public float CameraTiltDeg { get; set; }
    }
}
