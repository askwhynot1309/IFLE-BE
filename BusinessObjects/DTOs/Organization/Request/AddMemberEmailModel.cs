using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Organization.Request
{
    public class AddMemberEmailModel
    {
        public string Email { get; set; } = null!;

        public string HtmlBody { get; set; } = null!;

    }
}
