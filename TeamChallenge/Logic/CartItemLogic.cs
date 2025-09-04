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
        private readonly ILogger<CartItemLogic> _logger;

        public CartItemLogic(RepositoryFactory factory, ILogger<CartItemLogic> logger)
        {
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
            _logger = logger;
        }

        public async Task<IResponse> CreateCartItemAsync(CreateCartItemRequest dto)
        {
            try
            {
                await _cartItemRepository.CreateAsync(entity =>
                {
                    entity.ProductId = dto.ProductId;
                    entity.CartId = dto.CartId;
                    entity.Quantity = dto.Quantity;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart item");
                return new ServerErrorResponse("An error occurred while creating the cart item.");
            }
        }

        // how user can add multiple items to cart at once?
        public async Task<IResponse> CreateCartItemAsync(List<CreateCartItemRequest> list)
        {
            try
            {
                _logger.LogInformation("Started addition items to cart");
                var result = await _cartItemRepository.CreateCartItemAsync(list);
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

        public async Task<IResponse> UpdateCartItemAsync(int id, UpdateCartItemRequest dto)
        {
            try
            {
                var result = await _cartItemRepository.UpdateAsync(id, entity =>
                {
                    entity.Quantity = dto.Quantity;
                });

                if (!result)
                {
                    _logger.LogWarning("Cart item with Id = {id} not found for update.", id);
                    return new NotFoundResponse($"Cart item with Id = {id} not found");
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
