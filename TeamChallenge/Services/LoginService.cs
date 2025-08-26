using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public class LoginService : ILogin
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        

        public LoginService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IGenerateToken tokenService, IEmailSend emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        public async Task<IResponse> Login(LoginRequest request)
        {
            if (request == null)
            {
                return new UnauthorizedResponse("Invalid credentials");
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
                return new BadRequestResponse("User not found");
            }

            if (!user.EmailConfirmed)
            {
                return new UnauthorizedResponse("Confirm Email");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new UnauthorizedResponse("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);
            (string tokenString, DateTime Expires) = _tokenService.GenerateToken(user, roles);

            if (result.Succeeded)
            {
                return new LoginResponse(new LoginModel
                {
                    TokenString = tokenString,
                    Expires = Expires
                });
            }

            return new UnauthorizedResponse("All fields are require");
        }

        public async Task<IResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            return new OkResponse();
        }

        public async Task<IResponse> SignUp(SignUpRequest request)
        {
            if (request == null)
            {
                return new BadRequestResponse("Request is null");
            }

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return new BadRequestResponse("Fields must not be empty");
            }

            if (!EmailVerifyHelper.IsValidEmail(request.Email))
            {
                return new BadRequestResponse("Your email is not valid");
            }

            var user = new UserEntity { UserName = request.Username, Email = request.Email, SentEmailTime = DateTime.Now };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");

                var roles = await _userManager.GetRolesAsync(user);
                await SendConfirmLetter(request.Email, request.ClientUrl, user);

                return new OkResponse("User registered successfully.Please check your email to confirm your account.");
            }
            return new BadRequestResponse(string.Join("\n", result.Errors.Select(x => x.Description)));
        }        

        public async Task<IResponse> ResendEmailConfirmation(string email, string clientUrl)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new BadRequestResponse("Email address is required.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new OkResponse("If an account exists for that email, a confirmation link has been resent.");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return new BadRequestResponse("Your email address is already confirmed.");
            }

            var cooldownTime = TimeSpan.FromMinutes(1);
            if ((DateTime.UtcNow - user.SentEmailTime) < cooldownTime)
            {
                var remainingTime = cooldownTime - (DateTime.UtcNow - user.SentEmailTime);
                return new BadRequestResponse($"Please wait {remainingTime.Seconds} seconds before trying to resend the email.");
            }

            await SendConfirmLetter(email, clientUrl, user);
            user.SentEmailTime = DateTime.UtcNow;
            return new OkResponse("A new confirmation email has been sent. Please check your inbox.");
        }

        public async Task<IResponse> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                return new BadRequestResponse("Invalid confirmation link.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new BadRequestResponse("Invalid user email.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                return new OkResponse("Thank you for confirming your email. You can now log in.");
            }

            return new BadRequestResponse("Error confirming your email. The link may be invalid or expired.");
        }

        private async Task SendConfirmLetter(string email, string clientUrl, UserEntity user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
                {
                    {"token", token },
                    {"email", email }
                };

            var callback = QueryHelpers.AddQueryString(clientUrl + "api/auth/confirm-email", param);

            string emailBody = $"Hello! You've received a confirmation link. Please click here to confirm your email: <a href='{HtmlEncoder.Default.Encode(callback)}'>Click here</a>";

            await _emailSender.SendEmail(email, emailBody);
        }
    }
}
