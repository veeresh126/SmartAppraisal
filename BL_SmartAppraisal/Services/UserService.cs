using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;

namespace BL_SmartAppraisal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET ALL USERS
        public async Task<List<UserDetail>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // GET USER BY DATABASE ID
        public async Task<UserDetail?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        // CREATE USER
        public async Task<UserDetail> CreateAsync(UserDetail user)
        {
            return await _userRepository.CreateAsync(user);
        }

        // UPDATE USER
        public async Task<bool> UpdateAsync(UserDetail user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        // DELETE USER
        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        // LOGIN USING USER ID + PASSWORD
        public async Task<UserDetail?> LoginAsync(
            string userId,
            string password)
        {
            var users =
                await _userRepository.GetAllAsync();

            return users.FirstOrDefault(x =>
                x.UserId == userId &&
                x.Password == password &&
                x.IsActive);
        }

        // GET USER BY EMAIL
        public async Task<UserDetail?> GetByEmailAsync(
            string email)
        {
            return await _userRepository
                .GetByEmailAsync(email);
        }

        // AUTHENTICATE USING EMAIL + PASSWORD
        public async Task<UserDetail?> AuthenticateAsync(
            string email,
            string password)
        {
            var user =
                await _userRepository
                    .GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            if (user.Password != password)
            {
                return null;
            }

            return user;
        }
    }
}