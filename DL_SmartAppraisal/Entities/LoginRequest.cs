using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Entities
{
    public class LoginRequest
    {
        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
