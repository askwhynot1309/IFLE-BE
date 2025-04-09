using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Organization.Response
{
    public class OrganizationUserReponseModel
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string Privilege { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
