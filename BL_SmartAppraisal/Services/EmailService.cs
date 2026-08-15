using BL_SmartAppraisal.Interfaces;
using BL_SmartAppraisal.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BL_SmartAppraisal.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendLoginNotificationAsync(
            string recipientEmail,
            string userName)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            email.To.Add(
                MailboxAddress.Parse(recipientEmail));

            email.Subject = "SmartAppraisal Login Notification";

            email.Body = new BodyBuilder
            {
                HtmlBody = $"""
                    <html>
                    <body>
                        <h2>SmartAppraisal Login Notification</h2>

                        <p>Hello {userName},</p>

                        <p>
                            You have successfully logged into
                            your SmartAppraisal account.
                        </p>

                        <p>
                            <b>Login Time:</b> {DateTime.Now}
                        </p>

                        <p>
                            If this login was not performed by you,
                            please contact the administrator.
                        </p>

                        <br />

                        <p>
                            Regards,<br/>
                            SmartAppraisal Team
                        </p>
                    </body>
                    </html>
                    """
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _emailSettings.Username,
                _emailSettings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}