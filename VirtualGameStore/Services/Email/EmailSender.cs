using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace VirtualGameStore.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridApiKey;

        public EmailSender(IOptions<SendGridSettings> sendGridSettingsOptions)
        {
            _sendGridApiKey = sendGridSettingsOptions.Value.ApiKey;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var client = new SendGridClient(_sendGridApiKey);

            var message = new SendGridMessage()
            {
                From = new EmailAddress("malare4555@haboty.com", "Virtual Game Store"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };

            message.AddTo(new EmailAddress(recipient));
            message.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(message);
            var responseBody = await response.DeserializeResponseBodyAsync();
        }
    }
}
