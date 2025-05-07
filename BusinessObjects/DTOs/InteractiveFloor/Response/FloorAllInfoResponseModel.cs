using BusinessObjects.DTOs.Organization.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.InteractiveFloor.Response
{
    public class FloorAllInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Length { get; set; }

        public bool IsPublic { get; set; }

        public string Status { get; set; } = null!;

        public OrganizationAllInfoResponseModel? OrganizationInfo { get; set; }
    }
}
