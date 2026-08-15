using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_SmartAppraisal.Interfaces
{
    public interface IEmailService
    {
        Task SendLoginNotificationAsync(
            string recipientEmail,
            string userName);
    }
}
