using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Response
{
    public class UserOwnInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? AvatarUrl { get; set; }
    }
}
