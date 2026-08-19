namespace BL_SmartAppraisal.Interfaces
{
    public interface IOtpService
    {
        string GenerateOtp();

        string HashOtp(string otp);

        bool VerifyOtp(string otp, string storedHash);
    }
}