using DL_SmartAppraisal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_SmartAppraisal.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDetail>> GetAllAsync();

        Task<UserDetail?> GetByIdAsync(int id);

        Task<UserDetail> CreateAsync(UserDetail user);

        Task<bool> UpdateAsync(UserDetail user);

        Task<bool> DeleteAsync(int id);
    }
}
