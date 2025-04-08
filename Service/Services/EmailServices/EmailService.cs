using BusinessObjects.DTOs.Organization.Request;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.EmailServices
{
    public class EmailService : IEmailService
    {

        public async Task<bool> SendEmail(string Email, string Subject, string Html)
        {
            try
            {
                var toEmail = Email;
                string from = "interactivefloor.ifle@gmail.com";
                string pass = "wknx ugfz chjl sjac";
                MimeMessage message = new();
                message.From.Add(MailboxAddress.Parse(from));
                message.Subject = "[IFLE] " + Subject;
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = Html
                };
                using MailKit.Net.Smtp.SmtpClient smtp = new();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(from, pass);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailList(List<AddMemberEmailModel> models, string Subject)
        {
            try
            {
                string from = "interactivefloor.ifle@gmail.com";
                string pass = "wknx ugfz chjl sjac";
                MimeMessage message = new();
                using MailKit.Net.Smtp.SmtpClient smtp = new();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(from, pass);
                foreach (var model in models)
                {
                    message.From.Add(MailboxAddress.Parse(from));
                    message.Subject = "[IFLE] " + Subject;
                    message.To.Add(MailboxAddress.Parse(model.Email));
                    message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = model.HtmlBody
                    };
                    await smtp.SendAsync(message);
                }
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
