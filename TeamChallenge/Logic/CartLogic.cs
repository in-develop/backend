using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CartResponses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    [Authorize]
    public class CartLogic : ICartLogic
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartLogic> _logger;
        private readonly UserManager<UserEntity> _userManager;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

        public CartLogic(RepositoryFactory factory, ILogger<CartLogic> logger,
            UserManager<UserEntity> userManager, ICartItemLogic cartItemLogic, IHttpContextAccessor httpContextAccessor, ICartItemRepository cartItemRepository)
        {
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<IResponse> DeleteCartAsync(int id)
        {
            try
            {
                var result = await _cartRepository.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogError($"Error delete cart. ID: {id}");
                    return new NotFoundResponse($"An error occurred while deleting the cart. ID: {id}");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting cart ID: {id}");
                return new ServerErrorResponse("An error occurred while deleting the cart");
            }
        }

        public async Task<IResponse> GetCart()
        {
            try
            {
                int cartId = 0;
                if (!CartHelper.GetCartId(out cartId, HttpContext, _cartRepository, _logger))
                {
                    return new UnauthorizedResponse("User claims are missing or invalid.");
                }

                var result = await _cartItemRepository.GetWithProductsAndCartAsync(cartId);
                return new GetCartItemsResponse(result);
            }
            catch
            {
                _logger.LogError("An error occurred while finging the user id");
                return new ServerErrorResponse("An error occurred while finging the user id");
            }
        }
    }
}
