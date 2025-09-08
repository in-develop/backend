using System.Security.Claims;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CartItemResponses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CartItemLogic : ICartItemLogic
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartItemLogic> _logger;

        private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

        public CartItemLogic(RepositoryFactory factory, ILogger<CartItemLogic> logger, IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cartRepository = cartRepository;
        }

        public async Task<IResponse> CreateCartItemAsync(CreateCartItemRequest dto)
        {
            try
            {
                int cartId = 0;
                if (!CartHelper.GetCartId(out cartId, HttpContext, _cartRepository, _logger))
                {
                    return new UnauthorizedResponse("User claims are missing or invalid.");
                }

                await _cartItemRepository.CreateAsync(entity =>
                {
                    entity.ProductId = dto.ProductId;
                    entity.Quantity = dto.Quantity;
                    entity.CartId = cartId;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart item");
                return new ServerErrorResponse("An error occurred while creating the cart item.");
            }
        }

        public async Task<IResponse> CreateCartItemAsync(List<CreateCartItemRequest> list)
        {
            try
            {
                int cartId = 0;
                if (!CartHelper.GetCartId(out cartId, HttpContext, _cartRepository, _logger))
                {
                    return new UnauthorizedResponse("User claims are missing or invalid.");
                }

                _logger.LogInformation("Started addition items to cart");
                var result = await _cartItemRepository.CreateCartItemAsync(list, cartId);
                if (result)
                {
                    _logger.LogInformation("Addition completed");
                    return new OkResponse();
                }

                _logger.LogError("Addition is failed");
                return new ServerErrorResponse("Addition is failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the list of cart items.");
                return new ServerErrorResponse("An error occurred while creating the list of cart items.");
            }
        }

        public async Task<IResponse> DeleteCartItemAsync(int id)
        {
            try
            {
                var result = await _cartItemRepository.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Cart item with Id = {id} not found for deletion.", id);
                    return new NotFoundResponse($"Cart item with Id = {id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cart item");
                return new ServerErrorResponse("An error occurred while deleting the cart item.");
            }
        }

        public async Task<IResponse> GetCartItemAsync(int id)
        {
            try
            {
                var result = await _cartItemRepository.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogError($"Cart item not found ID : {id}");
                    return new NotFoundResponse($"Cart item not found ID : {id}");
                }

                return new GetCartItemResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving cart item {id}");
                return new ServerErrorResponse("An error occurred while retrieving the cart item.");
            }
        }

        public async Task<IResponse> UpdateCartItemAsync(UpdateCartItemRequest dto)
        {
            try
            {
                int cartId = 0;
                if (!CartHelper.GetCartId(out cartId, HttpContext, _cartRepository, _logger))
                {
                    return new UnauthorizedResponse("User claims are missing or invalid.");
                }

                var id = _cartItemRepository.GetFilteredAsync(x => x.CartId == cartId && x.ProductId == dto.ProductId).Result.FirstOrDefault()!.Id;
                var result = await _cartItemRepository.UpdateAsync(id, entity =>
                {
                    entity.Quantity = dto.Quantity;
                });

                if (!result)
                {
                    _logger.LogWarning("Cart item with Id = {id} not found for update.", cartId);
                    return new NotFoundResponse($"Cart item with Id = {cartId} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return new ServerErrorResponse("An error occurred while updating the cart item.");
            }
        }
    }
}
