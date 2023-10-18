using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestTaskData.DbModels;
using TestTaskData.Models;
using TestTaskData.Repositories.ImageRepository;

namespace TestTask.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                await imageRepository.UploadImage(model.ImageData);

                await imageRepository.CreateImage(new Image { Title = model.Title, UserId = Guid.Parse(userId) });
            }
            catch
            {
                return BadRequest("Error while uploading image");
            }

            return Ok();
        }
    }
}
