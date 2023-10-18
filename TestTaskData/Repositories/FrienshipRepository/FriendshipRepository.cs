using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.Data;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.FrienshipRepository
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly TestTaskDbContext _context;

        public FriendshipRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task AddFriendship(Friendship friendship)
        {
            User userA = await _context.Users.Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == friendship.UserAId);
            User userB = await _context.Users.FirstOrDefaultAsync(x => x.Id == friendship.UserBId);

            if (userA == null && userB == null) throw new Exception("Wrong users!");

            if (userA.Friends != null && userA.Friends.Contains(friendship))
            {
                throw new Exception("This user is already your friend!");

            }


            userA.Friends ??= new List<Friendship>();

            userA.Friends.Add(friendship);

            await _context.AddAsync(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> GetFriendsImages(Friendship friendship)
        {
            User userA = await _context.Users.Include(x => x.Images).Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == friendship.UserAId);
            User userB = await _context.Users.Include(x => x.Images).Include(x => x.Friends).FirstOrDefaultAsync(x => x.Id == friendship.UserBId);



            if (userA == null && userB == null) throw new Exception("Wrong users!");

            friendship = await _context.Friendships.FirstOrDefaultAsync(x => x.UserAId == userA.Id && x.UserBId == userB.Id);

            if (userA.Friends == null || !userA.Friends.Contains(friendship))
            {
                throw new Exception("This user is not your friend!");

            }

            if (userB.Images == null) throw new Exception("This user has not images!");

            return userB.Images.ToList();

        }

        public async Task RemoveFriendship(Guid id)
        {
            _context.Remove(id);

            await _context.SaveChangesAsync();
        }
    }
}
