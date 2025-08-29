using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CartLogic : ICartLogic
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ILogger<CartLogic> _logger;

        public CartLogic(RepositoryFactory factory, ILogger<CartLogic> logger)
        {
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _logger = logger;
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
        }

        public async Task<IResponse> CreateCartAsync(CreateCartRequest dto)
        {
            try
            {
                var cartItems = new List<CartItemEntity>();
                foreach (var item in dto.CartItems)
                {
                    var cartItem = _cartItemRepository.CreateAsync(entity =>
                    {
                        entity.ProductId = item.ProductId;
                        entity.Quantity = item.Quantity;
                    });

                    cartItems.Add(new CartItemEntity
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                await _cartRepository.CreateAsync(entity =>
                {
                    entity.UserId = dto.UserId;
                    entity.CartItems = cartItems;
                });


                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cart");
                return new ServerErrorResponse("An error occurred while creating the cart.");
            }
        }

        public async Task<IResponse> DeleteCartAsync(int id)
        {
            try
            {
                var result = await _cartRepository.DeleteAsync(id);
                if (!result)
                {
                    return new ServerErrorResponse("An error occurred while deleting the cart");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cart");
                return new ServerErrorResponse("An error occurred while deleting the cart");
            }

        }

        public async Task<IResponse> GetByIdCartAsync(int id)
        {
            try
            {
                await _cartRepository.GetByIdAsync(id);
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finging the cart by id");
                return new ServerErrorResponse("An error occurred while finging the cart by id");
            }
        }

        public async Task<IResponse> UpdateCartAsync(int id, UpdateCartRequest dto)
        {
            try
            {
                var result = await _cartRepository.UpdateAsync(id, entity =>
                {
                    entity.CartItems = dto.CartItems;
                });

                if (!result)
                {
                    return new ServerErrorResponse("An error occurred while updating the cart");
                }
                return new OkResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error find by id cart");
                return new ServerErrorResponse("An error occurred while updating the cart");
            }
        }
    }
}
