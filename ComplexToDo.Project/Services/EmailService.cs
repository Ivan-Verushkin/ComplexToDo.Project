using ComplexToDo.Project.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ComplexToDo.Project.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> _emailSettings)
        {
            this._emailSettings = _emailSettings.Value;   
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
