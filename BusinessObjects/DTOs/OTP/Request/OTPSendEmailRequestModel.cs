using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.OTP.Request
{
    public class OTPSendEmailRequestModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Please input your email")]
        public string Email { get; set; } = null!;
    }
}
