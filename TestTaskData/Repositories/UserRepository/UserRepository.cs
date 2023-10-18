using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.Data;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.UserRepository
{
    public class UserRepository:IUserRepository
    {
        private readonly TestTaskDbContext _context;

        public UserRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.Include(x => x.Images).Include(x => x.Friends).ToListAsync();
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
