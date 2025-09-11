using System.Security.Claims;
using TeamChallenge.Repositories;

namespace TeamChallenge.Helpers
{
    public static class CartHelper
    {
        public static bool GetCartId(out int cartId, HttpContext httpContext, ICartRepository cartRepository, ILogger logger)
        {
            cartId = 0;
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("User id claim is missing from the JWT token.");
                return false;
            }

            var cart = cartRepository.GetCartByUserId(userId).Result;

            if (cart == null)
            {
                logger.LogError("Cart is not exitst for user: {user}", userId);
                return false;
            }

            cartId = cart.Id;
            return true;
        }
    }
}
