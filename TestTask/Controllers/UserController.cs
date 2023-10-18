using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestTaskData.DbModels;
using TestTaskData.Models;
using TestTaskData.OtherServices;
using TestTaskData.Repositories.UserRepository;


namespace TestTask.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {

            User? user = (await userRepository.GetAllUsers()).FirstOrDefault(x => x.UserName == model.Username);

            if (user != null)
            {
                return BadRequest("Username is already taken.");
            }

            // Hash the password
            byte[] passwordHash, passwordSalt;
            AuthService.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            var newUser = new User
            {
                UserName = model.Username,
                PasswordHash = Convert.ToBase64String(passwordHash),
                Salt = Convert.ToBase64String(passwordSalt)
            };

            await userRepository.CreateUser(newUser);

            return StatusCode(201);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel model)
        {


            string accessToken;

            User? user = (await userRepository.GetAllUsers()).FirstOrDefault(x => x.UserName == model.Username); 


            if (user == null)
            {
                return BadRequest("User does not exists.");
            }

            bool IsPasswordCorrect = AuthService.VerifyPasswordHash(model.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.Salt));

            if (!IsPasswordCorrect)
            {
                return BadRequest("Password is wrong!");
            }




            accessToken = AuthService.GenerateAccessToken(user.Id.ToString(), user.UserName);





            return Ok(accessToken);

        }

        [HttpGet]
        [Authorize]

        public async Task<IEnumerable<User>> GetUsersWithImagesAndFriendsAsync()
        {
            return await userRepository.GetAllUsers();
        }



    }
}
