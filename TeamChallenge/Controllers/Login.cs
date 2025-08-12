using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Models;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogin _login;
        private readonly IGenerateToken _tokenService;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, ILogin login, IGenerateToken tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _login = login;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            if (request == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var user = new User();
            if (_login.IsValidEmail(request.UsernameOrEmail))
            {
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            }

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            (string tokenString, DateTime Expires) = _tokenService.GenerateToken(user, roles);

            if (result.Succeeded)
            {
                return Ok(new LoginResponse
                {
                    IsSucceded = true,
                    TokenString = tokenString,
                    Expires = Expires
                });
            }

            return BadRequest("All fields are require");
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var q = ModelState;

            var user = new User { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(result);
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return Ok(new Response { IsSucceded = true });
        }
    }
}
