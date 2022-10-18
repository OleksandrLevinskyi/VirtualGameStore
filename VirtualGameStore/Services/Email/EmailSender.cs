using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace VirtualGameStore.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettingsOptions)
        {
            _emailSettings = emailSettingsOptions.Value;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSettings.SenderEmail),
                Subject = subject,
                Body = builder.ToMessageBody()
            };

            email.To.Add(MailboxAddress.Parse(recipient));

            using var smtp = new SmtpClient();

            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);

            smtp.Authenticate(_emailSettings.SenderEmail, _emailSettings.Password);

            var result = await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
