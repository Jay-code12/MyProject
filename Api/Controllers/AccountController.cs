using Api.Dto.ccount;
using Api.Models;
using Api.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService jwtService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(JwtService jwtService, SignInManager<User> signInManager, UserManager<User> 
           userManager)
        {
            this.jwtService = jwtService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<UserDto> RefreshUserToken()
        {
            var user = await userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email) ? .Value) ;

            return createApplicationUserDto(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
             var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized("invalid username or email");
            }
            if(user.EmailConfirmed == false)
            {
                return Unauthorized("email not confirmed");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!result.Succeeded)
            {
                return Unauthorized("invalid username or password");
            }

            return createApplicationUserDto(user);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            if(await checkEmailEsist(model.Email))
            {
                return BadRequest("an account is already using the ema");
            }

            var userToadd = new User
            {
                FIrstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                UserName = model.Email.ToLower(),
                Email = model.Email.ToLower(),
                EmailConfirmed = true

            };

            var result = await userManager.CreateAsync(userToadd, model.Password);
            if(!result.Succeeded) 
            {
                return BadRequest();
            }

            return Ok("account successfully created");

        }


        #region private helper method

        private async Task<bool> checkEmailEsist(string email)
        {
            return await userManager.Users.AnyAsync(c => c.Email == email.ToLower());
        }
        private UserDto createApplicationUserDto(User user)
        {
            return new UserDto
            {
                firstName = user.FIrstName,
                lastName = user.FIrstName,
                jwt = jwtService.createJwt(user),
            };
        }
        #endregion
    }
}
