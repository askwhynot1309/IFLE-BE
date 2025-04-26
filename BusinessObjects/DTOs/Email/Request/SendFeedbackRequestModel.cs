using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Email.Request
{
    public class SendFeedbackRequestModel
    {
        [Required(ErrorMessage = "Vui lòng điền tên của bạn.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng điền email của bạn để chúng tôi có thể liên lạc.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng điền feedback hoặc những câu hỏi bạn muốn gửi về cho chúng tôi.")]
        public string Content { get; set; } = null!;
    }
}
