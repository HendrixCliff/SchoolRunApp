using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;

namespace SchoolRunApp.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.User, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}
