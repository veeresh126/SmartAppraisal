using BL_SmartAppraisal.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BL_SmartAppraisal.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            return RandomNumberGenerator
                .GetInt32(100000, 1000000)
                .ToString();
        }

        public string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(otp);

            var hash = sha256.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }

        public bool VerifyOtp(
            string otp,
            string storedHash)
        {
            var generatedHash = HashOtp(otp);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(generatedHash),
                Convert.FromHexString(storedHash));
        }
    }
}