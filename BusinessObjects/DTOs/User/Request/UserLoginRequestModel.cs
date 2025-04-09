using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class UserLoginRequestModel
    {
        [Required(ErrorMessage = "Hãy nhập email của bạn!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Hãy nhập mật khẩu của bạn!")]
        public string Password { get; set; } = null!;
    }
}
