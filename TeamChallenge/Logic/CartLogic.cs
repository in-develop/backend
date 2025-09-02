using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CartResponses;
using TeamChallenge.Repositories;

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

                if (!await _cartRepository.CreateCartItemsBulk(dto.CartItems))
                {
                    return new ServerErrorResponse("An error occurred while adding items to the cart.");
                }

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

        public async Task<IResponse> GetByIdCartAsync(int id)
        {
            try
            {
                var cartEntity = await _cartRepository.GetByIdAsync(id);
                if (cartEntity != null)
                {
                    return new GetСartResponse(cartEntity);
                }
                else
                {
                    _logger.LogWarning($"Cart not found with ID: {id}");
                    return new NotFoundResponse($"Cart not found with ID: {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while finging the cart by id = {id}");
                return new ServerErrorResponse($"An error occurred while finging the cart id = {id}");
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
