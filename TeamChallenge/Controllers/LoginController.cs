using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        private static readonly Dictionary<string, DateTime> _emailCooldowns = new Dictionary<string, DateTime>();

        public LoginController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IGenerateToken tokenService, IEmailSend emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<ActionResult<IDataResponse<LoginModel>>> Login([FromForm] LoginRequest request)
        {
            if (request == null)
            {
                return Unauthorized(new ErrorResponse("Invalid credentials"));
            }

            var user = new UserEntity();
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
                return BadRequest(new ErrorResponse("User not found"));
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized(new ErrorResponse("Confirm Email"));
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized(new ErrorResponse("Invalid credentials"));

            var roles = await _userManager.GetRolesAsync(user);
            (string tokenString, DateTime Expires) = _tokenService.GenerateToken(user, roles);

            if (result.Succeeded)
            {
                return Ok(new LoginResponse(new LoginModel
                {
                    TokenString = tokenString,
                    Expires = Expires
                }));
            }

            return BadRequest(new ErrorResponse("All fields are require"));
        }

        [HttpPost("signup")]
        public async Task<ActionResult<IResponse>> SignUp([FromForm] SignUpRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorResponse("Request is null"));
            }

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new ErrorResponse("Fields must not be empty"));
            }

            if (!EmailVerifyHelper.IsValidEmail(request.Email))
            {
                return BadRequest(new ErrorResponse("Your email is not valid"));
            }

            var user = new UserEntity { UserName = request.Username, Email = request.Email };
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
                    return BadRequest(new ErrorResponse("Could not generate confirmation link."));
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

                return Ok(new OkResponse("User registered successfully.Please check your email to confirm your account."));
            }
            return BadRequest(new ErrorResponse(string.Join("\n", result.Errors.Select(x => x.Description))));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<IResponse>> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new OkResponse());
        }

        [HttpGet("confirm/email")]
        public async Task<ActionResult<IResponse>> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest(new ErrorResponse("Invalid confirmation link."));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponse("Invalid user ID."));
            }
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
            
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                return Ok(new OkResponse("Thank you for confirming your email. You can now log in."));
            }
            
            return BadRequest(new ErrorResponse("Error confirming your email. The link may be invalid or expired."));            
        }

        [HttpPost("resend/email/confirmation")]
        public async Task<ActionResult<IResponse>> ResendEmailConfirmation([FromForm] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ErrorResponse("Email address is required."));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(new OkResponse("If an account exists for that email, a confirmation link has been resent."));
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new ErrorResponse("Your email address is already confirmed."));
            }

            var cooldownTime = TimeSpan.FromMinutes(1);
            if (_emailCooldowns.ContainsKey(email) && (DateTime.UtcNow - _emailCooldowns[email]) < cooldownTime)
            {
                var remainingTime = cooldownTime - (DateTime.UtcNow - _emailCooldowns[email]);
                return BadRequest(new ErrorResponse($"Please wait {remainingTime.Seconds} seconds before trying to resend the email."));
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Login",
                new { userId = user.Id, code = encodedCode },
                protocol: Request.Scheme);

            if (callbackUrl == null)
            {
                return BadRequest(new ErrorResponse("Could not generate confirmation link."));
            }

            string emailBody = $"Hello! You've received a new confirmation link. Please click here to confirm your email: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click here</a>";
            await _emailSender.SendEmail(email, emailBody);

            _emailCooldowns[email] = DateTime.UtcNow;

            return Ok(new OkResponse("A new confirmation email has been sent. Please check your inbox."));
        }
    }
}
