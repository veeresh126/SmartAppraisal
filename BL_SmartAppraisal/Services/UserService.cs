using BL_SmartAppraisal.Interfaces;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_SmartAppraisal.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDetail>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserDetail?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<UserDetail> CreateAsync(UserDetail user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> UpdateAsync(UserDetail user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }

}
