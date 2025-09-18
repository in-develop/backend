using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Logic;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;
using TeamChallenge.StaticData;

namespace TeamChallenge.Services
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        private readonly ICartRepository _cartRepository;
        private readonly IProductLogic _productLogic;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ILogger<LoginService> _logger;
        private readonly IConfiguration _configuration;

        public LoginService(
            SignInManager<UserEntity> signInManager,
            UserManager<UserEntity> userManager,
            IGenerateToken tokenService,
            IEmailSend emailSender,
            IProductLogic productLogic,
            ILogger<LoginService> logger,
            RepositoryFactory factory,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _logger = logger;
            _productLogic = productLogic;
            _configuration = configuration;
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
        }

        public async Task<IResponse> Login(TCLoginRequest request)
        {
            try
            {
                UserEntity user;
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
                    _logger.LogWarning("User not confirmed email");
                    return new UnauthorizedResponse("Confirm Email");
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Invalid login attempt for user: {username}", request.UsernameOrEmail);
                    return new UnauthorizedResponse("Invalid credentials");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var cart = await _cartRepository.GetCartByUserId(user.Id);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for user: {username}", request.UsernameOrEmail);
                    return new ServerErrorResponse("Cart not found. Please try again later.");
                }
                string tokenString = _tokenService.GenerateToken(user, roles, request.RememberMe, cart.Id);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Login attempt failed due to missing fields for user: {username}", request.UsernameOrEmail);
                    return new UnauthorizedResponse("All fields are require");
                }

                _logger.LogInformation("User {username} logged in successfully.", request.UsernameOrEmail);
                return new LoginResponse(new LoginResponseModel
                {
                    TokenString = tokenString,
                });
                
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
                if (!EmailVerifyHelper.IsValidEmail(request.Email))
                {
                    _logger.LogWarning("Invalid email format during SignUp: {email}", request.Email);
                    return new BadRequestResponse("Your email is not valid");
                }

                UserEntity user;

                _logger.LogInformation("Attempting to find user by email: {email}", request.Email);
                user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogInformation("Attempting to find user by username: {username}", request.Username);
                    user = await _userManager.FindByNameAsync(request.Username);
                }

                if (user != null)
                {
                    _logger.LogWarning("User already exists with provided email or username: {email}/{username}", request.Email, request.Username);
                    return new BadRequestResponse("User already exists.");
                }

                if (request.CartItems != null && request.CartItems.Count > 0)
                {
                    var response = await _productLogic.CheckIfProductsExists(request.CartItems.Select(x => x.ProductId).ToArray());

                    if (!response.IsSuccess)
                    {
                        return response;
                    }
                }

                user = new UserEntity { UserName = request.Username, Email = request.Email, SentEmailTime = DateTime.Now };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogError("User creation failed for {username}. Errors: {errors}", request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new BadRequestResponse(string.Join("\n", result.Errors.Select(x => x.Description)));
                }

                _logger.LogInformation("User {username} created a new account with password.", request.Username);
                await _userManager.AddToRoleAsync(user, "Member");

                var roles = await _userManager.GetRolesAsync(user);

                _logger.LogInformation("Creating cart for user {0}, ID: {1}", request.Username, user.Id);

                var cart = await _cartRepository.CreateAsync(cart =>
                {
                    cart.UserId = user.Id;
                });

                if (request.CartItems != null && request.CartItems.Count > 0)
                {
                    await _cartItemRepository.CreateManyAsync(request.CartItems.Count, cartItems =>
                    {
                        for (int i = 0; i < request.CartItems.Count; i++)
                        {
                            cartItems[i].ProductId = request.CartItems[i].ProductId;
                            cartItems[i].Quantity = request.CartItems[i].Quantity;
                            cartItems[i].CartId = cart.Id;
                        }
                    });
                }

                await SendConfirmLetter(request.Email, GlobalConsts.ClientUrl, user);
                user.SentEmailTime = DateTime.UtcNow;

                return new OkResponse("User registered successfully.Please check your email to confirm your account.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during SignUp for user: {username} \n{ex}", request?.Username, ex.Message);
                return new ServerErrorResponse("An error occurred during registration. Please try again later.");

            }
        }


        public async Task<IResponse> ResendEmailConfirmation(ResendEmailConfirmationRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    _logger.LogWarning("ResendEmailConfirmation called with empty email.");
                    return new BadRequestResponse("Email address is required.");
                }

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("No user found with email: {email}", request.Email);
                    return new OkResponse("If an account exists for that email, a confirmation link has been resent.");
                }

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    _logger.LogInformation("Email already confirmed for user with email: {email}", request.Email);
                    return new BadRequestResponse("Your email address is already confirmed.");
                }

                var cooldownTime = TimeSpan.FromMinutes(int.Parse(_configuration["Sender:Timeout"]!));
                if ((DateTime.Now - user.SentEmailTime) < cooldownTime)
                {
                    _logger.LogWarning("ResendEmailConfirmation attempted too soon for email: {email}", request.Email);
                    var remainingTime = cooldownTime - (DateTime.UtcNow - user.SentEmailTime);
                    return new BadRequestResponse($"Please wait {remainingTime.Seconds} seconds before trying to resend the email.");
                }

                await SendConfirmLetter(request.Email, GlobalConsts.ClientUrl, user);
                user.SentEmailTime = DateTime.Now;
                await _userManager.UpdateAsync(user);
                _logger.LogInformation("Resent confirmation email to: {email}", request.Email);
                return new OkResponse("A new confirmation email has been sent. Please check your inbox.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resending confirmation email to: {email}\n{ex}", request.Email, ex.Message);
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
