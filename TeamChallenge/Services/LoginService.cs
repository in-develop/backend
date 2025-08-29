using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests.Login;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        private readonly ILogger<LoginService> _logger;

        public LoginService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IGenerateToken tokenService, IEmailSend emailSender, ILogger<LoginService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<IResponse> Login(TCLoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("Login request is null.");
                    return new UnauthorizedResponse("Invalid credentials");
                }

                var user = new UserEntity();
                if (EmailVerifyHelper.IsValidEmail(request.UsernameOrEmail))
                {
                    _logger.LogInformation("Attempting to find user by email: {email}", request.UsernameOrEmail);
                    user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
                }
                else
                {
                    _logger.LogInformation("Attempting to find user by username: {username}", request.UsernameOrEmail);
                    user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
                }

                if (user == null)
                {
                    _logger.LogWarning("User not found for login attempt with identifier: {identifier}", request.UsernameOrEmail);
                    return new BadRequestResponse("User not found");
                }

                if (!user.EmailConfirmed)
                {
                    return new UnauthorizedResponse("Confirm Email");
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Invalid login attempt for user: {username}", request.UsernameOrEmail);
                    return new UnauthorizedResponse("Invalid credentials");
                }

                var roles = await _userManager.GetRolesAsync(user);
                (string tokenString, DateTime Expires) = _tokenService.GenerateToken(user, roles);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {username} logged in successfully.", request.UsernameOrEmail);
                    return new LoginResponse(new LoginModel
                    {
                        TokenString = tokenString,
                        Expires = Expires
                    });
                }

                _logger.LogWarning("Login attempt failed due to missing fields for user: {username}", request.UsernameOrEmail);
                return new UnauthorizedResponse("All fields are require");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {username}\n{ex}", request?.UsernameOrEmail, ex.Message);
                return new ServerErrorResponse("An error occurred during login. Please try again later.");
            }
        }

        public async Task<IResponse> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.\n{ex}", ex.Message);
                return new ServerErrorResponse("An error occurred during logout. Please try again later.");
            }
        }

        public async Task<IResponse> SignUp(SignUpRequest request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogWarning("SignUp request is null.");
                    return new BadRequestResponse("Request is null");
                }

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
                {
                    _logger.LogWarning("SignUp request contains empty fields.");
                    return new BadRequestResponse("Fields must not be empty");
                }

                if (!EmailVerifyHelper.IsValidEmail(request.Email))
                {
                    _logger.LogWarning("Invalid email format during SignUp: {email}", request.Email);
                    return new BadRequestResponse("Your email is not valid");
                }

                var user = new UserEntity { UserName = request.Username, Email = request.Email, SentEmailTime = DateTime.Now };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {username} created a new account with password.", request.Username);
                    await _userManager.AddToRoleAsync(user, "Member");

                    var roles = await _userManager.GetRolesAsync(user);
                    await SendConfirmLetter(request.Email, request.ClientUrl, user);

                    user.SentEmailTime = DateTime.UtcNow;

                    return new OkResponse("User registered successfully.Please check your email to confirm your account.");
                }

                _logger.LogWarning("User creation failed for {username}. Errors: {errors}", request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                return new BadRequestResponse(string.Join("\n", result.Errors.Select(x => x.Description)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during SignUp for user: {username} \n{ex}", request?.Username, ex.Message);
                return new ServerErrorResponse("An error occurred during registration. Please try again later.");
            }
        }

        public async Task<IResponse> ResendEmailConfirmation(string email, string clientUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("ResendEmailConfirmation called with empty email.");
                    return new BadRequestResponse("Email address is required.");
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("No user found with email: {email}", email);
                    return new OkResponse("If an account exists for that email, a confirmation link has been resent.");
                }

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    _logger.LogInformation("Email already confirmed for user with email: {email}", email);
                    return new BadRequestResponse("Your email address is already confirmed.");
                }

                var cooldownTime = TimeSpan.FromMinutes(1);
                if ((DateTime.UtcNow - user.SentEmailTime) < cooldownTime)
                {
                    _logger.LogWarning("ResendEmailConfirmation attempted too soon for email: {email}", email);
                    var remainingTime = cooldownTime - (DateTime.UtcNow - user.SentEmailTime);
                    return new BadRequestResponse($"Please wait {remainingTime.Seconds} seconds before trying to resend the email.");
                }

                await SendConfirmLetter(email, clientUrl, user);
                user.SentEmailTime = DateTime.UtcNow;
                _logger.LogInformation("Resent confirmation email to: {email}", email);
                return new OkResponse("A new confirmation email has been sent. Please check your inbox.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resending confirmation email to: {email}\n{ex}", email, ex.Message);
                return new ServerErrorResponse("An error occurred while resending the confirmation email. Please try again later.");
            }
        }

        public async Task<IResponse> ConfirmEmail(string email, string token)
        {
            try
            {
                if (email == null || token == null)
                {
                    _logger.LogWarning("ConfirmEmail called with null email or token.");
                    return new BadRequestResponse("Invalid confirmation link.");
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("No user found with email during email confirmation: {email}", email);
                    return new BadRequestResponse("Invalid user email.");
                }
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Email confirmed successfully for user with email: {email}", email);
                    user.EmailConfirmed = true;
                    return new OkResponse("Thank you for confirming your email. You can now log in.");
                }

                _logger.LogWarning("Error confirming your email. The link may be invalid or expired");
                return new BadRequestResponse("Error confirming your email. The link may be invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during email confirmation for email: {email}\n{ex}", email, ex.Message);
                return new ServerErrorResponse("An error occurred during email confirmation. Please try again later.");
            }
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
