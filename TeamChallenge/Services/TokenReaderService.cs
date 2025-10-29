using System.Security.Claims;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public class TokenReaderService(
        ILogger<TokenReaderService> logger,
        IHttpContextAccessor httpContextAccessor)
        : ITokenReaderService
    {
        private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

        public IResponse GetCartId()
        {
            var cartId = _httpContext.User.FindFirstValue("CartId");

            if (!int.TryParse(cartId, out var cartIdInt))
            {
                logger.LogWarning("User claims does not contain cart id or it is invalid.");
                return new UnauthorizedResponse("User claims does not contain cart id or it is invalid.");
            }

            return new GetCartIdResponse(cartIdInt);
        }

        public IResponse GetUserId()
        {
            var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("User claims does not contain user id or it is invalid.");
                return new UnauthorizedResponse("User claims does not contain user id or it is invalid.");
            }

            return new GetUserIdResponse(userId);
        }
    }
}
