using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class UserUpdateRequestModel
    {
        public string FullName { get; set; } = null!;

        public string? AvatarUrl { get; set; }
    }
}
