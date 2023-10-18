using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskData.DbModels;

namespace TestTaskData.Repositories.FrienshipRepository
{
    public interface IFriendshipRepository
    {
        public Task AddFriendship(Friendship friendship);
        public Task RemoveFriendship(Guid id);

        public Task<IEnumerable<Image>> GetFriendsImages(Friendship friendship);
    }
}
