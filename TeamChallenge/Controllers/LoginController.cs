using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
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
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        private static readonly Dictionary<string, DateTime> _emailCooldowns = new Dictionary<string, DateTime>();

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, IGenerateToken tokenService, IEmailSend emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] Models.Requests.LoginRequest request)
        {
            if (request == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var user = new User();
            if (EmailVerifyHelper.IsValidEmail(request.UsernameOrEmail))
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

            if (!user.IsEmailConfirmed)
            {
                return Unauthorized("Confirm Email");
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
                return BadRequest("Request is null");
            }

            if(string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Fields must not be empty");
            }

            var user = new User { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                var roles = await _userManager.GetRolesAsync(user);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Login", 
                new { userId = user.Id, code = encodedCode },
                protocol: Request.Scheme);

                if (callbackUrl == null)
                {
                    return BadRequest("Could not generate confirmation link.");
                }
                string emailBody = $"Hello! You've received a confirmation link. Please click here to confirm your email: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click here</a>";
                await _emailSender.SendEmail(request.Email, emailBody);
                if (_emailCooldowns.ContainsKey(request.Email))
                {
                    _emailCooldowns[request.Email] = DateTime.UtcNow;
                }
                else
                {
                    _emailCooldowns.Add(request.Email, DateTime.UtcNow);
                }// bad

                return Ok(new Response
                {
                    IsSucceded = true,
                    Message = "User registered successfully.Please check your email to confirm your account."
                });
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

        [HttpGet("confirm/email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Invalid confirmation link.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Invalid user ID.");
            }
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
            
            if (result.Succeeded)
            {
                user.IsEmailConfirmed = true;
                return Ok("Thank you for confirming your email. You can now log in.");
            }
            
            return BadRequest("Error confirming your email. The link may be invalid or expired.");
            
        }

        [HttpPost("resend/email/confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromForm] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email address is required.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(new { Message = "If an account exists for that email, a confirmation link has been resent." });
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new { Message = "Your email address is already confirmed." });
            }

            var cooldownTime = TimeSpan.FromMinutes(1);
            if (_emailCooldowns.ContainsKey(email) && (DateTime.UtcNow - _emailCooldowns[email]) < cooldownTime)
            {
                var remainingTime = cooldownTime - (DateTime.UtcNow - _emailCooldowns[email]);
                return BadRequest(new { Message = $"Please wait {remainingTime.Seconds} seconds before trying to resend the email." });
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code = encodedCode },
                protocol: Request.Scheme);

            if (callbackUrl == null)
            {
                return BadRequest("Could not generate confirmation link.");
            }

            string emailBody = $"Hello! You've received a new confirmation link. Please click here to confirm your email: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click here</a>";
            await _emailSender.SendEmail(email, emailBody);

            _emailCooldowns[email] = DateTime.UtcNow;

            return Ok(new { Message = "A new confirmation email has been sent. Please check your inbox." });
        }
    }
}
