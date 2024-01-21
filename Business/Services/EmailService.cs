using MailKit.Net.Smtp;
using MimeKit;

namespace Business.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = body,
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.your-email-provider.com", 587, false);
                await client.AuthenticateAsync("your-email@example.com", "your-email-password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
