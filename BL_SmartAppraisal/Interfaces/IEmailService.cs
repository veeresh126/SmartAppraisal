namespace BL_SmartAppraisal.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string htmlBody,
            string textBody);

        Task SendLoginNotificationAsync(
            string recipientEmail,
            string userName);

        Task SendOtpAsync(
            string recipientEmail,
            string userName,
            string otp);

        Task SendCaseStudySubmittedAsync(
            string recipientEmail,
            string userName,
            int caseStudyId);

        Task SendCaseStudyReviewedAsync(
            string recipientEmail,
            string userName,
            int caseStudyId,
            bool approved,
            string? reviewComment);
    }

}