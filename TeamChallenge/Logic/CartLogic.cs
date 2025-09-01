using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;
using TeamChallenge.Models.Responses.CartResponses;

namespace TeamChallenge.Logic
{
    [Authorize]
    public class CartLogic : ICartLogic
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemLogic _cartItemLogic;
        private readonly ILogger<CartLogic> _logger;
        private readonly UserManager<UserEntity> _userManager;

        public CartLogic(RepositoryFactory factory, ILogger<CartLogic> logger,
            UserManager<UserEntity> httpContextAccessor, ICartItemLogic cartItemLogic)
        {
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _logger = logger;
            _cartItemLogic = cartItemLogic;
            _userManager = httpContextAccessor;
        }

        public async Task<IResponse> CreateCartAsync(CreateCartRequest dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);

                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", dto.UserId);
                    return new ServerErrorResponse("User not found");
                }

                if (!await _cartRepository.IsCartExist(user.Id))
                {
                    await _cartRepository.CreateAsync(entity =>
                    {
                        entity.UserId = dto.UserId;
                    });
                }

                var result = new List<CartItemEntity>();
                foreach (var item in dto.CartItems)
                {
                    await _cartItemLogic.CreateCartItemAsync(new CreateCartItemRequest
                    {
                        ProductId = item.ProductId,
                        CartId = item.CartId,
                        Quantity = item.Quantity
                    });

                    result.Add(new CartItemEntity
                    {
                        CartId = item.CartId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }


                return new CreateCartListResponse(result);
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
                return new GetСartResponse(await _cartRepository.GetByIdAsync(id));
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
                    _logger.LogError("Error update cart");
                    return new ServerErrorResponse("An error occurred while updating the cart");
                }

                return new GetСartResponse(await _cartRepository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error find by id cart");
                return new ServerErrorResponse("An error occurred while updating the cart");
            }
        }
    }
}
