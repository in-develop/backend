using Microsoft.AspNetCore.Identity;
using TeamChallenge.Models.Entities;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IGenerateToken _tokenService;
        private readonly IEmailSend _emailSender;
        private readonly ICartLogic _cartLogic;
        private readonly ICartItemLogic _cartItemLogic;
        private readonly ILogger<LoginService> _logger;

        public UserLogic(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, ILogger<LoginService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> CheckIfUserExists(string id)
        {
            return (await _userManager.FindByIdAsync(id)) != null;
        }
    }
}
