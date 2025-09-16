using System.Security.Claims;
using TeamChallenge.Models.Responses;
using TeamChallenge.StaticData;

namespace TeamChallenge.Services
{
    public class TokenReaderService : ITokenReaderService
    {
        private readonly ILogger<TokenReaderService> _logger;
        private HttpContext _httpContext;
        public TokenReaderService(
            ILogger<TokenReaderService> logger,
            IHttpContextAccessor httpContextAccessor)
        {

            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public IResponse GetCartId()
        {
            var cartId = _httpContext.User.FindFirstValue(CustomClaimTypes.CartId);

            if (!int.TryParse(cartId, out int cartIdInt))
            {
                _logger.LogWarning("User claims does not contain cart id or it is invalid.");
                return new UnauthorizedResponse("User claims does not contain cart id or it is invalid.");
            }

            return new GetCartIdResponse(cartIdInt);
        }

        public IResponse GetUserId()
        {
            var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User claims does not contain user id or it is invalid.");
                return new UnauthorizedResponse("User claims does not contain user id or it is invalid.");
            }

            return new GetUserIdResponse(userId);
        }
    }
}
