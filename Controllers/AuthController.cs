using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZ_Walk.Models.DTO;
using NZ_Walk.Repositories;

namespace NZ_Walk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDtos registerRequestDtos)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDtos.Username,
                Email = registerRequestDtos.Username
            };

            //check if user already exist in DB
            if(_userManager.FindByEmailAsync(IdentityUser.Email) != null)
                return BadRequest("Email is already registered.");
            if (_userManager.FindByEmailAsync(IdentityUser.UserName) != null)
                return BadRequest("Username is already registered.");

            var IdentityResult = await _userManager.CreateAsync(IdentityUser,registerRequestDtos.Password);

            if(IdentityResult.Succeeded)
            {
                if (registerRequestDtos.Roles != null && registerRequestDtos.Roles.Any())
                {
                    IdentityResult result = await _userManager.AddToRolesAsync(IdentityUser, registerRequestDtos.Roles);
                }

                if (IdentityResult.Succeeded)
                {
                    return Ok("User was registered! Please login.");
                }
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDtos loginRequestDtos)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDtos.Username);
            if(user !=null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDtos.Password); 
                if(checkPasswordResult)
                {
                    //Get roles for this user
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        var JwtToken = _tokenRepository.CreatJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = JwtToken,
                        };
                        return Ok(response);
                    }
                    return Ok();
                }
            }

            return BadRequest("username or password incorrect");
        }

        
    }
}
