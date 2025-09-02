using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;
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

        public async Task<IResponse> UpdateCartItemAsync(int id, UpdateCartItemRequest dto)
        {
            try
            {
                var result = await _cartItemRepository.UpdateAsync(id, entity =>
                {
                    entity.ProductId = dto.ProductId;
                    entity.CartId = dto.CartId;
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
