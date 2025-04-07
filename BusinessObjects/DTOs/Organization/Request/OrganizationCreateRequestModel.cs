using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Organization.Request
{
    public class OrganizationCreateRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tổ chức.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
