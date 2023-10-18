using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestTaskData.DbModels;
using TestTaskData.Models;
using TestTaskData.Repositories.FrienshipRepository;
using TestTaskData.Repositories.ImageRepository;

namespace TestTask.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipRepository friendshipRepository;

        public FriendshipController(IFriendshipRepository friendshipRepository)
        {
            this.friendshipRepository = friendshipRepository;
        }

        [HttpPost]
        public async Task MakeFriend([FromBody] FriendModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await friendshipRepository.AddFriendship(new Friendship { UserAId = Guid.Parse(userId), UserBId = model.FriendId });
        }

        [HttpGet]
        public async Task<IEnumerable<Image>> WatchFriendsImages([FromBody] FriendModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await friendshipRepository.GetFriendsImages(new Friendship { UserAId = Guid.Parse(userId), UserBId = model.FriendId });
        }

        [HttpPost]
        [Route("{id}")]
        public async Task RemoveFreind([FromRoute] Guid id)
        {
            await friendshipRepository.RemoveFriendship(id);


        }
    }
}
