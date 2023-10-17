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
        private readonly SignInManager<User> signInManager;

        public UserController(IUserRepository userRepository, SignInManager<User> signInManager)
        {
            this.userRepository = userRepository;
            this.signInManager = signInManager;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel model)
        {


            string accessToken;

            User? user = (await userRepository.GetAllUsers()).FirstOrDefault(x => x.UserName == model.Username); ;


            if (user == null)
            {
                return BadRequest("User does not exists.");
            }

            string passwordHash = AuthService.CreatePasswordHash(model.Password, out passwordHash, out Convert.FromBase64String(user.Salt)); ;

            if (passwordHash != user.Password!.Trim())
            {
                throw new SignInFailedException("Wrong Password!");
            }


            try
            {

                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    throw new SignInFailedException("Username and Email can not be empty!");
                }

                accessToken = GenerateAccessToken(user.Id.ToString(), user.UserName, user.Email);
                string refreshToken = GenerateRefreshToken();

                await _repo.UpdateDbObjectAsync(user.Id, new User { RefreshToken = refreshToken });

            }
            catch
            {
                throw new SignInFailedException("Error while creating jwt token!");
            }


            return accessToken;

        }

        [HttpGet]
        [Authorize]

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await userRepository.GetAllUsers();
        }
    }
}
