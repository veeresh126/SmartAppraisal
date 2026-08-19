using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SmartAppraisal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public UsersController(
            IUserService userService,
            IOtpService otpService,
            IEmailService emailService)
        {
            _userService = userService;
            _otpService = otpService;
            _emailService = emailService;
        }


        // =========================================================
        // GET ALL USERS
        // GET: /api/Users
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users =
                await _userService.GetAllAsync();

            return Ok(users);
        }


        // =========================================================
        // GET USER BY ID
        // GET: /api/Users/5
        // =========================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user =
                await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // =========================================================
        // CREATE USER
        // POST: /api/Users
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            UserDetail user)
        {
            var result =
                await _userService.CreateAsync(user);

            return Ok(result);
        }


        // =========================================================
        // UPDATE USER
        // PUT: /api/Users/5
        // =========================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UserDetail user)
        {
            user.Id = id;

            var result =
                await _userService.UpdateAsync(user);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        // =========================================================
        // DELETE USER
        // DELETE: /api/Users/5
        // =========================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _userService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        // =========================================================
        // LOGIN
        // POST: /api/Users/login
        // =========================================================

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(
                    "User ID and Password are required.");
            }

            var user =
                await _userService.LoginAsync(
                    request.UserId,
                    request.Password);

            if (user == null)
            {
                return Unauthorized(
                    "Invalid User ID or Password.");
            }


            // ---------------------------------------------------------
            // Store login session
            // ---------------------------------------------------------

            HttpContext.Session.SetString(
                "UserId",
                user.UserId);

            HttpContext.Session.SetInt32(
                "UserDbId",
                user.Id);


            // ---------------------------------------------------------
            // Login email
            // ---------------------------------------------------------

            try
            {
                await _emailService
                    .SendLoginNotificationAsync(
                        user.Email,
                        user.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Login email failed: " +
                    ex.Message);
            }


            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                name = user.Name,
                roleId = user.RoleId
            });
        }


        // =========================================================
        // FORGOT PASSWORD
        // POST: /api/Users/forgot-password
        // =========================================================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(
                    "Email is required.");
            }


            var email =
                request.Email.Trim();


            // ---------------------------------------------------------
            // Find user
            // ---------------------------------------------------------

            var user =
                await _userService
                    .GetByEmailAsync(email);


            // Don't reveal whether the email exists.
            if (user == null)
            {
                return Ok(new
                {
                    message =
                        "If the email exists, an OTP has been sent."
                });
            }


            // ---------------------------------------------------------
            // Generate OTP
            // ---------------------------------------------------------

            var otp =
                _otpService.GenerateOtp();


            // Store HASH in database
            var otpHash =
                _otpService.HashOtp(otp);


            // OTP valid for 5 minutes
            var expiry =
                DateTime.UtcNow.AddMinutes(5);


            // ---------------------------------------------------------
            // Save OTP information
            // ---------------------------------------------------------

            user.PasswordResetToken =
                otpHash;

            user.PasswordResetTokenExpiry =
                expiry;


            var updated =
                await _userService
                    .UpdateAsync(user);


            if (!updated)
            {
                return StatusCode(
                    500,
                    "Unable to save password reset information.");
            }


            // ---------------------------------------------------------
            // Save reset information in session
            // ---------------------------------------------------------

            HttpContext.Session.SetString(
                "PasswordResetEmail",
                user.Email);

            HttpContext.Session.SetInt32(
                "PasswordResetOtpAttempts",
                0);

            HttpContext.Session.SetInt32(
                "PasswordResetOtpVerified",
                0);


            // ---------------------------------------------------------
            // Send OTP email
            // ---------------------------------------------------------

            try
            {
                await _emailService
                    .SendOtpAsync(
                        user.Email,
                        user.Name,
                        otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "OTP email failed: " +
                    ex.Message);

                // IMPORTANT:
                // The OTP is already stored in DB.
                //
                // Return the actual error so the frontend
                // can tell you that SMTP/email is the problem.

                return StatusCode(
                    500,
                    "OTP was generated and saved, but the email could not be sent. " +
                    "Check your SMTP/EmailSettings configuration. " +
                    "Error: " + ex.Message);
            }


            return Ok(new
            {
                message =
                    "OTP has been sent to your registered email."
            });
        }


        // =========================================================
        // VERIFY OTP
        // POST: /api/Users/verify-otp
        // =========================================================

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(
            VerifyOtpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(
                    "Email and OTP are required.");
            }


            var user =
                await _userService
                    .GetByEmailAsync(
                        request.Email.Trim());


            if (user == null)
            {
                return BadRequest(
                    "Invalid password reset request.");
            }


            // ---------------------------------------------------------
            // Check token
            // ---------------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                    user.PasswordResetToken))
            {
                return BadRequest(
                    "OTP request has expired. Please request a new OTP.");
            }


            // ---------------------------------------------------------
            // Check expiry
            // ---------------------------------------------------------

            if (!user.PasswordResetTokenExpiry.HasValue)
            {
                return BadRequest(
                    "OTP request has expired. Please request a new OTP.");
            }


            if (DateTime.UtcNow >
                user.PasswordResetTokenExpiry.Value)
            {
                await ClearPasswordResetData(user);

                return BadRequest(
                    "OTP has expired.");
            }


            // ---------------------------------------------------------
            // Check attempts
            // ---------------------------------------------------------

            var attempts =
                HttpContext.Session.GetInt32(
                    "PasswordResetOtpAttempts") ?? 0;


            if (attempts >= 5)
            {
                await ClearPasswordResetData(user);

                return BadRequest(
                    "Maximum OTP attempts exceeded.");
            }


            HttpContext.Session.SetInt32(
                "PasswordResetOtpAttempts",
                attempts + 1);


            // ---------------------------------------------------------
            // Verify OTP
            // ---------------------------------------------------------

            bool isValid;

            try
            {
                isValid =
                    _otpService.VerifyOtp(
                        request.Otp,
                        user.PasswordResetToken);
            }
            catch
            {
                isValid = false;
            }


            if (!isValid)
            {
                return BadRequest(
                    "Invalid OTP.");
            }


            // ---------------------------------------------------------
            // OTP verified
            // ---------------------------------------------------------

            HttpContext.Session.SetString(
                "PasswordResetEmail",
                user.Email);

            HttpContext.Session.SetInt32(
                "PasswordResetOtpVerified",
                1);


            return Ok(new
            {
                message =
                    "OTP verified successfully."
            });
        }


        // =========================================================
        // RESEND OTP
        // POST: /api/Users/resend-otp
        // =========================================================

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(
            ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(
                    "Email is required.");
            }


            var user =
                await _userService
                    .GetByEmailAsync(
                        request.Email.Trim());


            if (user == null)
            {
                return Ok(new
                {
                    message =
                        "If the email exists, an OTP has been sent."
                });
            }


            // ---------------------------------------------------------
            // Generate new OTP
            // ---------------------------------------------------------

            var otp =
                _otpService.GenerateOtp();

            var otpHash =
                _otpService.HashOtp(otp);

            var expiry =
                DateTime.UtcNow.AddMinutes(5);


            // ---------------------------------------------------------
            // Save new OTP
            // ---------------------------------------------------------

            user.PasswordResetToken =
                otpHash;

            user.PasswordResetTokenExpiry =
                expiry;


            var updated =
                await _userService
                    .UpdateAsync(user);


            if (!updated)
            {
                return StatusCode(
                    500,
                    "Unable to save new OTP.");
            }


            // ---------------------------------------------------------
            // Reset session
            // ---------------------------------------------------------

            HttpContext.Session.SetString(
                "PasswordResetEmail",
                user.Email);

            HttpContext.Session.SetInt32(
                "PasswordResetOtpAttempts",
                0);

            HttpContext.Session.SetInt32(
                "PasswordResetOtpVerified",
                0);


            // ---------------------------------------------------------
            // Send email
            // ---------------------------------------------------------

            try
            {
                await _emailService
                    .SendOtpAsync(
                        user.Email,
                        user.Name,
                        otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Resend OTP email failed: " +
                    ex.Message);

                return StatusCode(
                    500,
                    "New OTP was saved, but email could not be sent. " +
                    "Check your SMTP settings. " +
                    "Error: " + ex.Message);
            }


            return Ok(new
            {
                message =
                    "New OTP has been sent."
            });
        }


        // =========================================================
        // RESET PASSWORD
        // POST: /api/Users/reset-password
        // =========================================================

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.NewPassword) ||
                string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return BadRequest(
                    "All fields are required.");
            }


            if (request.NewPassword !=
                request.ConfirmPassword)
            {
                return BadRequest(
                    "Passwords do not match.");
            }


            // ---------------------------------------------------------
            // Check OTP verification
            // ---------------------------------------------------------

            var verified =
                HttpContext.Session.GetInt32(
                    "PasswordResetOtpVerified");


            if (verified != 1)
            {
                return Unauthorized(
                    "Please verify OTP first.");
            }


            // ---------------------------------------------------------
            // Check email against session
            // ---------------------------------------------------------

            var sessionEmail =
                HttpContext.Session.GetString(
                    "PasswordResetEmail");


            if (!string.Equals(
                    sessionEmail,
                    request.Email.Trim(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    "Invalid password reset request.");
            }


            // ---------------------------------------------------------
            // Get user
            // ---------------------------------------------------------

            var user =
                await _userService
                    .GetByEmailAsync(
                        request.Email.Trim());


            if (user == null)
            {
                return BadRequest(
                    "Unable to reset password.");
            }


            // ---------------------------------------------------------
            // Update password
            // ---------------------------------------------------------

            user.Password =
                request.NewPassword;


            // Clear reset token
            user.PasswordResetToken =
                null;

            user.PasswordResetTokenExpiry =
                null;


            var result =
                await _userService
                    .UpdateAsync(user);


            if (!result)
            {
                return StatusCode(
                    500,
                    "Unable to update password.");
            }


            // ---------------------------------------------------------
            // Clear session
            // ---------------------------------------------------------

            ClearOtpSession();


            return Ok(new
            {
                message =
                    "Password reset successfully."
            });
        }


        // =========================================================
        // CLEAR OTP SESSION
        // =========================================================

        private void ClearOtpSession()
        {
            HttpContext.Session.Remove(
                "PasswordResetEmail");

            HttpContext.Session.Remove(
                "PasswordResetOtpAttempts");

            HttpContext.Session.Remove(
                "PasswordResetOtpVerified");
        }


        // =========================================================
        // CLEAR DATABASE RESET DATA
        // =========================================================

        private async Task ClearPasswordResetData(
            UserDetail user)
        {
            user.PasswordResetToken =
                null;

            user.PasswordResetTokenExpiry =
                null;

            await _userService.UpdateAsync(user);

            ClearOtpSession();
        }
    }
}