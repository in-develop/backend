using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Login;
using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Requests.Login;
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
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ILogger<LoginService> _logger;

        public LoginService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IGenerateToken tokenService, IEmailSend emailSender, ILogger<LoginService> logger, ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _logger = logger;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
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

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {username} logged in successfully.", request.UsernameOrEmail);
                    return new LoginResponse(new LoginModel
                    {
                        TokenString = tokenString,
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

        [Authorize]
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

                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    _logger.LogWarning("Email already in use during SignUp: {email}", request.Email);
                    return new BadRequestResponse("Email is already in use");
                }

                if (await _userManager.FindByNameAsync(request.Username) != null)
                {
                    _logger.LogWarning("Username already in use during SignUp: {username}", request.Username);
                    return new BadRequestResponse("Username is already in use");
                }

                var user = new UserEntity { UserName = request.Username, Email = request.Email, SentEmailTime = DateTime.Now };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogWarning("User creation failed for {username}. Errors: {errors}", request.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return new BadRequestResponse(string.Join("\n", result.Errors.Select(x => x.Description)));
                }

                _logger.LogInformation("User {username} created a new account with password.", request.Username);
                await _userManager.AddToRoleAsync(user, "Member");
                var roles = await _userManager.GetRolesAsync(user);
                bool isSucssess = await CreateCart(request, user);
                if (!isSucssess)
                {
                    _logger.LogError("Cart creation failed. Please try again later.");
                    await _userManager.DeleteAsync(user);
                    return new ServerErrorResponse("Cart creation failed. Please try again later.");
                }


                await SendConfirmLetter(request.Email, BaseClass.ClientUrl, user);
                user.SentEmailTime = DateTime.UtcNow;

                return new OkResponse("User registered successfully.Please check your email to confirm your account.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during SignUp for user: {username} \n{ex}", request?.Username, ex.Message);
                return new ServerErrorResponse("An error occurred during registration. Please try again later.");

            }
        }

        private async Task<bool> CreateCart(SignUpRequest request, UserEntity user)
        {
            _logger.LogInformation("Creating cart for user: {username}, ID: {id}", request.Username, user.Id);
            if (request.CartItems != null)  
            {
                await _cartRepository.CreateAsync(cart =>
                {
                    cart.UserId = user.Id;
                });

                var tempCart = await _cartRepository.GetCartByUserId(user.Id);
                if (tempCart == null)
                {
                    _logger.LogError("Cart creation failed for user: {username}, ID: {id}", request.Username, user.Id);
                    await _userManager.DeleteAsync(user);
                    return false;
                }
                var temoDto = new List<CreateCartItemRequest>();
                foreach (var cartItem in request.CartItems)
                {
                    temoDto.Add(new CreateCartItemRequest
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity
                    });
                }

                if (!await _cartItemRepository.CreateCartItemAsync(temoDto, tempCart.Id))
                {
                    _logger.LogError("Cart items creation failed for user: {username}, ID: {id}", request.Username, user.Id);
                    return false;
                }
            }
            else
            {
                await _cartRepository.CreateAsync(cart => 
                {
                    cart.UserId = user.Id;
                });
            }

            return true;
        }

        public async Task<IResponse> ResendEmailConfirmation(string email)
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

                await SendConfirmLetter(email, BaseClass.ClientUrl, user);
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
