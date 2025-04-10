using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class UserRegisterWithPwRequestModel
    {
        [Required(ErrorMessage = "Hãy nhập tên của bạn.")]
        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = "Hãy nhập email của bạn.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Hãy nhập mật khẩu của bạn.")]
        [Length(6, 14, ErrorMessage = "Mật khẩu phải có độ dài từ 6-14 ký tự.")]
        public string InputPassword { get; set; } = null!;
    }
}
