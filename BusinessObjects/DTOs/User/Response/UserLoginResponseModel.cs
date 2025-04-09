using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Response
{
    public class UserAuthResponseModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

}
