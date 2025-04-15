using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Ultis
{
    public static class HTMLEmailTemplate
    {
        public static string VerifyEmailOTP(string fullname, string OTP)
        {
            var html = $@"<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                             <div style='border: 1px solid #e0e0e0; padding: 20px; max-width: 600px; margin: auto;'>
                                 <p style='font-size: 16px;'>Kính gửi <strong>{fullname}</strong>,</p>
                                 <hr style='border: none; border-bottom: 1px solid #ccc; margin: 20px 0;'/>
                                 <p style='font-size: 14px;'>
                                    Mã xác nhận (OTP) của bạn là: <strong style='font-size: 18px; color: #d9534f;'>{OTP}</strong><br/>
                                    Vui lòng nhập mã này để xác nhận địa chỉ email và kích hoạt tài khoản của bạn.<br/>
                                    Sau khi xác nhận, tài khoản của bạn sẽ được kích hoạt và bạn có thể đăng nhập và sử dụng các dịch vụ của chúng tôi.<br/>
                                    Nếu bạn không yêu cầu xác nhận email, vui lòng bỏ qua email này hoặc <a href='...' style='color: #0066cc;'>liên hệ chúng tôi</a> ngay lập tức.
                                 </p>
                                 <p style='font-size: 14px;'>Đây là email tự động, vui lòng không trả lời email này.</p>
                                 <p style='font-size: 14px;'>Trân trọng,<br/><strong>IFLE</strong></p>
                             </div>
                         </div>";
            return html;
        }

        public static string SendingOTPEmail(string fullname, string otp, string purpose)
        {
            var html = $@"<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                             <div style='border: 1px solid #e0e0e0; padding: 20px; max-width: 600px; margin: auto;'>
                                 <p style='font-size: 16px;'>Kính gửi <strong>{fullname}</strong>,</p>
                                 <hr style='border: none; border-bottom: 1px solid #ccc; margin: 20px 0;'/>
                                 <p style='font-size: 14px;'>
                                    Bạn đã yêu cầu <strong>{purpose}</strong> vào lúc {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} +07.<br/>
                                    Đây là mã OTP của bạn: <strong style='font-size: 18px; color: #d9534f;'>{otp}</strong><br/>
                                    Nếu bạn không yêu cầu việc này, vui lòng liên hệ <a href='...' style='color: #0066cc;'>chúng tôi</a> ngay lập tức.
                                </p>
                                 <p style='font-size: 14px;'>Đây là email tự động, vui lòng không trả lời email này.</p>
                                 <p style='font-size: 14px;'>Trân trọng,<br/><strong>IFLE</strong></p>
                             </div>
                         </div>";
            return html;
        }

        public static string SendingNewPasswordEmail(string fullname, string newPassword)
        {
            var html = $@"<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                             <div style='border: 1px solid #e0e0e0; padding: 20px; max-width: 600px; margin: auto;'>
                                 <p style='font-size: 16px;'>Kính gửi <strong>{fullname}</strong>,</p>
                                 <hr style='border: none; border-bottom: 1px solid #ccc; margin: 20px 0;'/>
                                 <p style='font-size: 14px;'>
                                    Mật khẩu mới của bạn là: <strong style='font-size: 18px; color: #d9534f;'>{newPassword}</strong><br/>
                                    Vui lòng đảm bảo cập nhật mật khẩu sau khi đăng nhập để bảo mật tài khoản của bạn.<br/>
                                    Nếu bạn không yêu cầu việc này, vui lòng liên hệ <a href='...' style='color: #0066cc;'>chúng tôi</a> ngay lập tức.
                                </p>
                                 <p style='font-size: 14px;'>Đây là email tự động, vui lòng không trả lời email này.</p>
                                 <p style='font-size: 14px;'>Trân trọng,<br/><strong>IFLE</strong></p>
                             </div>
                         </div>";
            return html;
        }

        public static string SendingOrganizationInvitationEmail(string fullName, string email, string date, string organizationName)
        {
            var html = $@"<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                     <div style='border: 1px solid #e0e0e0; padding: 20px; max-width: 600px; margin: auto;'>
                         <p style='font-size: 16px;'>Kính gửi <strong>{fullName}</strong>,</p>
                         <hr style='border: none; border-bottom: 1px solid #ccc; margin: 20px 0;'/>
                         <p style='font-size: 14px;'>
                             Tài khoản của bạn có địa chỉ email <strong>{email}</strong> đã được thêm vào tổ chức 
                             <strong style='color: #5cb85c;'>{organizationName}</strong> vào thời gian <strong>{date}</strong>.
                         </p>
                         <p style='font-size: 14px;'>
                             Nếu bạn nhận ra tổ chức này, vui lòng bỏ qua email này.<br/>
                             <br/>
                             <strong style='color: #d9534f;'>Nếu bạn không phải là thành viên của tổ chức này hoặc không yêu cầu việc này, 
                             vui lòng liên hệ <a href='...' style='color: #0066cc;'>chúng tôi</a> để được hỗ trợ.</strong>
                         </p>
                         <p style='font-size: 14px;'>Đây là email tự động, vui lòng không trả lời email này.</p>
                         <p style='font-size: 14px;'>Trân trọng,<br/><strong>IFLE</strong></p>
                     </div>
                 </div>";
            return html;
        }

        public static string PaymentSuccessNotification(string fullname, string packageName, DateTime paymentTime)
        {
            var formattedDate = paymentTime.ToString("dd/MM/yyyy");

            var html = $@"<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                             <div style='border: 1px solid #e0e0e0; padding: 20px; max-width: 600px; margin: auto;'>
                                 <p style='font-size: 16px;'>Kính gửi <strong>{fullname}</strong>,</p>
                                 <hr style='border: none; border-bottom: 1px solid #ccc; margin: 20px 0;'/>
                                 <p style='font-size: 14px;'>
                                     Chúng tôi xin thông báo rằng thanh toán của bạn đã được xác nhận thành công khi mua gói {packageName} vào ngày <strong>{formattedDate}</strong>.
                                 </p>
                                 <p style='font-size: 14px;'>Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='...' style='color: #0066cc;'>liên hệ chúng tôi</a>.</p>
                                 <p style='font-size: 14px;'>Đây là email tự động, vui lòng không trả lời email này.</p>
                                 <p style='font-size: 14px;'>Trân trọng,<br/><strong>IFLE</strong></p>
                             </div>
                         </div>";
            return html;
        }

    }
}
