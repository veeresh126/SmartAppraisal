using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Entities
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
