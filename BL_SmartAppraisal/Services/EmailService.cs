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

        public EmailService(
            IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string htmlBody,
            string textBody)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            email.To.Add(
                MailboxAddress.Parse(recipientEmail));

            email.Subject = subject;

            var body = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = textBody
            };

            email.Body = body.ToMessageBody();

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

        public async Task SendLoginNotificationAsync(
            string recipientEmail,
            string userName)
        {
            await SendEmailAsync(
                recipientEmail,
                "SmartAppraisal Login Notification",

                $"""
                <html>
                <body>
                    <h2>SmartAppraisal Login Notification</h2>

                    <p>Hello {userName},</p>

                    <p>
                        You have successfully logged into
                        your SmartAppraisal account.
                    </p>

                    <p>
                        Login Time:
                        <b>{DateTime.Now}</b>
                    </p>

                    <p>
                        If this login was not performed by you,
                        please contact the administrator.
                    </p>

                    <p>
                        Regards,<br/>
                        SmartAppraisal Team
                    </p>
                </body>
                </html>
                """,

                $"""
                SmartAppraisal Login Notification

                Hello {userName},

                You have successfully logged into your
                SmartAppraisal account.

                Login Time: {DateTime.Now}

                If this login was not performed by you,
                please contact the administrator.

                SmartAppraisal Team
                """);
        }

        public async Task SendOtpAsync(
            string recipientEmail,
            string userName,
            string otp)
        {
            await SendEmailAsync(
                recipientEmail,
                "SmartAppraisal - Password Reset OTP",

                $"""
                <html>
                <body>

                    <h2>Password Reset</h2>

                    <p>Hello {userName},</p>

                    <p>
                        We received a request to reset
                        your SmartAppraisal password.
                    </p>

                    <p>Your OTP is:</p>

                    <h1>{otp}</h1>

                    <p>
                        This OTP is valid for
                        <b>5 minutes</b>.
                    </p>

                    <p>
                        Do not share this OTP with anyone.
                    </p>

                    <p>
                        Regards,<br/>
                        SmartAppraisal Team
                    </p>

                </body>
                </html>
                """,

                $"""
                SmartAppraisal Password Reset

                Hello {userName},

                Your password reset OTP is:

                {otp}

                This OTP is valid for 5 minutes.

                Do not share this OTP with anyone.

                SmartAppraisal Team
                """);
        }

        public async Task SendCaseStudySubmittedAsync(
            string recipientEmail,
            string userName,
            int caseStudyId)
        {
            await SendEmailAsync(
                recipientEmail,
                $"SmartAppraisal - Case Study #{caseStudyId} Submitted",

                $"""
                <html>
                <body>

                    <h2>Case Study Submitted</h2>

                    <p>Hello {userName},</p>

                    <p>
                        Your case study has been submitted successfully.
                    </p>

                    <p>
                        Case Study ID:
                        <b>{caseStudyId}</b>
                    </p>

                    <p>
                        The case study is now waiting for review.
                    </p>

                    <p>
                        SmartAppraisal Team
                    </p>

                </body>
                </html>
                """,

                $"""
                Case Study Submitted

                Hello {userName},

                Your case study has been submitted successfully.

                Case Study ID: {caseStudyId}

                The case study is now waiting for review.

                SmartAppraisal Team
                """);
        }

        public async Task SendCaseStudyReviewedAsync(
            string recipientEmail,
            string userName,
            int caseStudyId,
            bool approved,
            string? reviewComment)
        {
            var status =
                approved ? "Approved" : "Rejected";

            await SendEmailAsync(
                recipientEmail,
                $"SmartAppraisal - Case Study {status}",

                $"""
                <html>
                <body>

                    <h2>Case Study {status}</h2>

                    <p>Hello {userName},</p>

                    <p>
                        Your case study
                        <b>#{caseStudyId}</b>
                        has been <b>{status}</b>.
                    </p>

                    <p>
                        <b>Reviewer Comment:</b>
                    </p>

                    <p>
                        {reviewComment ?? "No comment provided."}
                    </p>

                    <p>
                        SmartAppraisal Team
                    </p>

                </body>
                </html>
                """,

                $"""
                Case Study {status}

                Hello {userName},

                Your case study #{caseStudyId}
                has been {status}.

                Reviewer Comment:
                {reviewComment ?? "No comment provided."}

                SmartAppraisal Team
                """);
        }
    }
}