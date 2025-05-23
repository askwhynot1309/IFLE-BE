﻿using BusinessObjects.DTOs.OTP.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.OTPServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/otp")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _otpService;

        public OTPController(IOTPService oTPService)
        {
            _otpService = oTPService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendOTP(OTPSendEmailRequestModel model)
        {
            await _otpService.CreateOTPCodeForEmail(model);
            return Ok("Mã OTP đã được gửi đến email của bạn.");
        }

        [HttpPut]
        [Route("verify-email")]
        public async Task<IActionResult> VerifyEmailOTP(OTPVerifyRequestModel model)
        {
            await _otpService.CheckOTPInVerifyAccount(model);
            return Ok("Xác thực mã OTP thành công.");
        }

        [HttpPut]
        [Route("forget-password")]
        public async Task<IActionResult> GetNewPasswword(OTPVerifyRequestModel model)
        {
            await _otpService.VerifyOTPToSendNewPassword(model);
            return Ok("Xác thực mã OTP thành công. Mật khẩu mới đã được gửi tới mail.");
        }
    }
}
