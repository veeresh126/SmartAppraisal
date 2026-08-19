using DL_SmartAppraisal.Data;
using DL_SmartAppraisal.Entities;
using DL_SmartAppraisal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_SmartAppraisal.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly SmartAppraisalDbContext _context;

        public UserRepository(SmartAppraisalDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDetail>> GetAllAsync()
        {
            return await _context.UserDetails.ToListAsync();
        }

        public async Task<UserDetail?> GetByIdAsync(int id)
        {
            return await _context.UserDetails
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserDetail> AddAsync(UserDetail user)
        {
            _context.UserDetails.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UpdateAsync(UserDetail user)
        {
            var existingUser = await _context.UserDetails.FindAsync(user.Id);

            if (existingUser == null)
            {
                return false;
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.UserDetails.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            _context.UserDetails.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDetail> CreateAsync(UserDetail user)
        {
            _context.UserDetails.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDetail?> GetByEmailAsync(string email)
        {
            return await _context.UserDetails
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == email.ToLower());
        }

    }

}
