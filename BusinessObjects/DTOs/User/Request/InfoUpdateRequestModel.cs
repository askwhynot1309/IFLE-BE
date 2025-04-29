using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class InfoUpdateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng không để trống tên người dùng.")]
        public string FullName { get; set; } = null!;

    }
}
