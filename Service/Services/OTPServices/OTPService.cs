using BusinessObjects.DTOs.OTP.Request;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.OTPRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.EmailServices;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.OTPServices
{
    public class OTPService : IOTPService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOTPRepository _otpCodeRepository;

        public OTPService(IUserRepository userRepository, IOTPRepository oTPCodeRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _otpCodeRepository = oTPCodeRepository;
            _emailService = emailService;
        }

        private string CreateNewOTPCode()
        {
            return new Random().Next(0, 999999).ToString("D6");
        }

        public async Task CreateOTPCodeForEmail(OTPSendEmailRequestModel model)
        {
            User currentUser = await _userRepository.GetUserByEmail(model.Email);

            if (currentUser == null)
            {
                throw new CustomException("Email bạn nhập không kết nối với tài khoản nào.");
            }

            var latestOTP = await _otpCodeRepository.GetLatestOTP(currentUser.Id);
            if (latestOTP != null && !latestOTP.IsUsed)
            {
                if ((DateTime.Now - latestOTP.CreatedAt).TotalMinutes < 2)
                {
                    throw new CustomException($"Không thể gữi mã OTP lúc này, vui lòng đợi sau {(120 - (DateTime.Now - latestOTP.CreatedAt).TotalSeconds).ToString("0.00")} giây để thử lại");
                }
            }

            string newOTP = CreateNewOTPCode();
            var htmlBody = HTMLEmailTemplate.SendingOTPEmail(currentUser.FullName, newOTP, "gửi OTP");
            bool sendEmailSuccess = await _emailService.SendEmail(model.Email, "Yêu cầu gửi OTP", htmlBody);
            if (!sendEmailSuccess)
            {
                throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
            }
            OTP newOTPCode = new OTP()
            {
                Id = Guid.NewGuid().ToString(),
                Code = newOTP,
                UserId = currentUser.Id,
                CreatedAt = DateTime.Now,
                IsUsed = false,
            };

            await _otpCodeRepository.Insert(newOTPCode);
        }

        public async Task CheckOTPInVerifyAccount(OTPVerifyRequestModel model)
        {
            User currentUser = await _userRepository.GetUserByEmail(model.Email);

            if (currentUser == null)
            {
                throw new CustomException("Email bạn nhập không kết nối với tài khoản nào.");
            }

            var latestOTP = await _otpCodeRepository.GetLatestOTP(currentUser.Id);

            if (latestOTP != null)
            {
                if ((DateTime.Now - latestOTP.CreatedAt).TotalMinutes > 30 || latestOTP.IsUsed)
                {
                    throw new CustomException("Mã OTP đã quá thời gian hoặc đã được sử dụng. Xin vui lòng nhập mã OTP mới nhất.");
                }

                if (latestOTP.Code.Equals(model.OTP))
                {
                    latestOTP.IsUsed = true;
                    currentUser.IsVerified = true;
                }
                else
                {
                    throw new CustomException("Mã OTP không hợp lệ.");
                }

                await _otpCodeRepository.Update(latestOTP);
                await _userRepository.Update(currentUser);
            }
        }
    }
}
