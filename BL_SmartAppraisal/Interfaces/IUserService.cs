using DL_SmartAppraisal.Entities;

namespace BL_SmartAppraisal.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDetail>> GetAllAsync();

        Task<UserDetail?> GetByIdAsync(int id);

        Task<UserDetail> CreateAsync(UserDetail user);

        Task<bool> UpdateAsync(UserDetail user);

        Task<bool> DeleteAsync(int id);

        Task<UserDetail?> LoginAsync(
            string userId,
            string password);

        Task<UserDetail?> GetByEmailAsync(
            string email);

        Task<UserDetail?> AuthenticateAsync(
            string email,
            string password);
    }
}