using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    [Authorize]
    public class CartLogic : ICartLogic
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartLogic> _logger;
        private HttpContext _httpContext;

        public CartLogic(
            RepositoryFactory factory, 
            ILogger<CartLogic> logger,
            ICartItemLogic cartItemLogic, 
            IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
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

        public async Task<IResponse> GetCartWithCartItems()
        {
            try
            {

                var response = await GetValidCart();

                if (!response.IsSuccess)
                {
                    return response;
                }

                var cart = (response as GetCartResponse).Data;

                return new GetCartItemsResponse(cart.CartItems.Select(x => 
                    new GetCartItemsResponseModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        Price = x.Product.Price,
                        CartId = x.CartId
                    }
                ));
            }
            catch
            {
                _logger.LogError("An error occurred while finging the user id");
                return new ServerErrorResponse("An error occurred while finging the user id");
            }
        }

        public async Task<IResponse> GetValidCart()
        {
            var cartId = _httpContext.User.FindFirstValue("CartId");

            if (int.TryParse(cartId, out int cartIdInt))
            {
                _logger.LogWarning("User claims does not contain cart id or it is invalid.");
                return new UnauthorizedResponse("User claims does not contain cart id or it is invalid.");
            }

            var cart = await _cartRepository.GetByIdAsync(cartIdInt);

            if (cart == null)
            {
                _logger.LogError("Cart not found. ID: {0}", cartIdInt);
                return new NotFoundResponse($"Cart not found. ID: {cartIdInt}");
            }

            return new DataResponse<CartEntity>(cart);
        }
    }
}
