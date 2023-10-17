using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(Guid id);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task CreateUser(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(Guid id);

    }
}
