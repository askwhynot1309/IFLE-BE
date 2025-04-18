using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Organization.Response
{
    public class OrganizationInfoResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int UserLimit { get; set; }

        public string Privilege { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

    }
}
