using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.EmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string Email, string Subject, string Html);
    }
}
