using BusinessObjects.DTOs.OTP.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.OTPServices
{
    public interface IOTPService
    {
        Task CreateOTPCodeForEmail(OTPSendEmailRequestModel model);
        Task CheckOTPInVerifyAccount(OTPVerifyRequestModel model);
    }
}
