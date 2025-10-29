using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Requests.Login;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Static_data;

namespace TeamChallenge.Services
{
    public class LoginService(
        SignInManager<UserEntity> signInManager,
        UserManager<UserEntity> userManager,
        IGenerateToken tokenService,
        IEmailSend emailSender,
        ILogger<LoginService> logger,
        IConfiguration configuration)
        : ILoginService
    {
        public async Task<IResponse> Login(TCLoginRequest request)
        {
            try
            {
                UserEntity? user;
                if (EmailVerifyHelper.IsValidEmail(request.UsernameOrEmail))
                {
                    logger.LogInformation("Attempting to find user by email: {email}", request.UsernameOrEmail);
                    user = await userManager.FindByEmailAsync(request.UsernameOrEmail);
                }
                else
                {
                    logger.LogInformation("Attempting to find user by username: {username}", request.UsernameOrEmail);
                    user = await userManager.FindByNameAsync(request.UsernameOrEmail);
                }

                if (user == null)
                {
                    logger.LogWarning("User not found for login attempt with identifier: {identifier}", request.UsernameOrEmail);
                    return new BadRequestResponse("User not found");
                }

                if (!user.EmailConfirmed)
                {
                    logger.LogWarning("User not confirmed email");
                    return new UnauthorizedResponse("Confirm Email");
                }

                var result = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    logger.LogWarning("Invalid login attempt for user: {username}", request.UsernameOrEmail);
                    return new UnauthorizedResponse("Invalid credentials");
                }

                var roles = await userManager.GetRolesAsync(user);

                var tokenString = tokenService.GenerateToken(user, roles, request.RememberMe);

                logger.LogInformation("User {username} logged in successfully.", request.UsernameOrEmail);
                return new LoginResponse(new LoginResponseModel
                {
                    TokenString = tokenString,
                });

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during login for user: {username}\n{ex}", request?.UsernameOrEmail, ex.Message);
                return new ServerErrorResponse("An error occurred during login. Please try again later.");
            }
        }

        public async Task<IResponse> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return new OkResponse();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during logout.\n{ex}", ex.Message);
                return new ServerErrorResponse("An error occurred during logout. Please try again later.");
            }
        }

        public async Task<IResponse> SignUp(SignUpRequest request)
        {
            try
            {
                if (!EmailVerifyHelper.IsValidEmail(request.Email))
                {
                    logger.LogWarning("Invalid email format during SignUp: {email}", request.Email);
                    return new BadRequestResponse("Your email is not valid");
                }

                logger.LogInformation("Attempting to find user by email: {email}", request.Email);
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    logger.LogInformation("Attempting to find user by username: {username}", request.Username);
                    user = await userManager.FindByNameAsync(request.Username);
                }

                if (user != null)
                {
                    logger.LogWarning("User already exists with provided email or username: {email}/{username}", request.Email, request.Username);
                    return new BadRequestResponse("User already exists.");
                }

                user = new UserEntity { UserName = request.Username, Email = request.Email };
                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                    logger.LogError("User creation failed for {username}. Errors: {errors}", request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new BadRequestResponse(string.Join("\n", result.Errors.Select(x => x.Description)));
                }

                logger.LogInformation("User {username} created a new account with password.", request.Username);
                await userManager.AddToRoleAsync(user, "Member");

                await SendConfirmLetter(request.Email, GlobalConstants.ClientUrl, user);
                user.SentEmailTime = DateTime.UtcNow;

                return new OkResponse("User registered successfully. Please check your email to confirm your account.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during SignUp for user: {username} \n{ex}", request?.Username, ex.Message);
                return new ServerErrorResponse("An error occurred during registration. Please try again later.");
            }
        }

        public async Task<IResponse> ResendEmailConfirmation(ResendEmailConfirmationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    logger.LogWarning("ResendEmailConfirmation called with empty email.");
                    return new BadRequestResponse("Email address is required.");
                }

                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    logger.LogWarning("No user found with email: {email}", request.Email);
                    return new OkResponse("If an account exists for that email, a confirmation link has been resent.");
                }

                if (await userManager.IsEmailConfirmedAsync(user))
                {
                    logger.LogInformation("Email already confirmed for user with email: {email}", request.Email);
                    return new BadRequestResponse("Your email address is already confirmed.");
                }

                var cooldownTime = TimeSpan.FromMinutes(int.Parse(configuration["Sender:Timeout"]!));
                if (DateTime.UtcNow - user.SentEmailTime < cooldownTime)
                {
                    logger.LogWarning("ResendEmailConfirmation attempted too soon for email: {email}", request.Email);
                    var remainingTime = cooldownTime - (DateTime.UtcNow - user.SentEmailTime);
                    return new BadRequestResponse($"Please wait {remainingTime.Seconds} seconds before trying to resend the email.");
                }

                await SendConfirmLetter(request.Email, GlobalConstants.ClientUrl, user);
                user.SentEmailTime = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
                logger.LogInformation("Resent confirmation email to: {email}", request.Email);
                return new OkResponse("A new confirmation email has been sent. Please check your inbox.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while resending confirmation email to: {email}\n{ex}", request.Email, ex.Message);
                return new ServerErrorResponse("An error occurred while resending the confirmation email. Please try again later.");
            }
        }

        public async Task<IResponse> ConfirmEmail(string? email, string? token)
        {
            try
            {
                if (email == null || token == null)
                {
                    logger.LogWarning("ConfirmEmail called with null email or token.");
                    return new BadRequestResponse("Invalid confirmation link.");
                }

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    logger.LogWarning("No user found with email during email confirmation: {email}", email);
                    return new BadRequestResponse("Invalid user email.");
                }
                var result = await userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    logger.LogInformation("Email confirmed successfully for user with email: {email}", email);
                    user.EmailConfirmed = true;
                    return new OkResponse("Thank you for confirming your email. You can now log in.");
                }

                logger.LogWarning("Error confirming your email. The link may be invalid or expired");
                return new BadRequestResponse("Error confirming your email. The link may be invalid or expired.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during email confirmation for email: {email}\n{ex}", email, ex.Message);
                return new ServerErrorResponse("An error occurred during email confirmation. Please try again later.");
            }
        }

        private async Task SendConfirmLetter(string email, string clientUrl, UserEntity user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?> { {"token", token }, {"email", email } };
            var callback = QueryHelpers.AddQueryString(clientUrl + "api/auth/confirm-email", param);
            var emailBody = $"Hello! You've received a confirmation link. Please click here to confirm your email: <a href='{HtmlEncoder.Default.Encode(callback)}'>Click here</a>";

            await emailSender.SendEmail(email, emailBody);
        }
    }
}
