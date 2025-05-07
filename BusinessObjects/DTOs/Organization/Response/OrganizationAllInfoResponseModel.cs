using BusinessObjects.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Organization.Response
{
    public class OrganizationAllInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int UserLimit { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Status { get; set; }

        public UserInfoResponeModel? OwnerInfo { get; set; }
    }
}
