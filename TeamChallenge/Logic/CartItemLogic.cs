using Azure;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CartItemLogic : ICartItemLogic
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartLogic _cartLogic;
        private readonly IProductLogic _productLogic;
        private readonly ILogger<CartItemLogic> _logger;

        public CartItemLogic(
            RepositoryFactory factory, 
            ILogger<CartItemLogic> logger, 
            ICartLogic cartLogic,
            IProductLogic productLogic)
        {
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
            _logger = logger;
            _cartLogic = cartLogic;
            _productLogic = productLogic;
        }

        public async Task<IResponse> CreateCartItemAsync(CreateCartItemRequest request)
        {
            try
            {   
                var response = await _productLogic.CheckIfProductsExists(request.ProductId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                response = await _cartLogic.GetCart();

                if (!response.IsSuccess)
                {
                    return response;
                }

                var cart = response.As<GetCartResponse>().Data;

                await _cartItemRepository.CreateAsync(entity =>
                {
                    entity.ProductId = request.ProductId;
                    entity.Quantity = request.Quantity;
                    entity.CartId = cart.Id;
                });

                return new OkResponse();
            }
            catch
            {
                return new ServerErrorResponse($"An error occurred while creating the cart item.");
            }
        }

        public async Task<IResponse> DeleteCartItemAsync(int id)
        {
            try
            {
                var result = await _cartItemRepository.DeleteAsync(id);

                if (!result)
                {
                    return new NotFoundResponse($"Cart item with Id = {id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse("An error occurred while deleting the cart item.");
            }
        }

        public async Task<IResponse> DeleteCartItemsFromCartAsync()
        {

            var response = await _cartLogic.GetCart();

            if (!response.IsSuccess)
            {
                return response;
            }

            var cart = response.As<GetCartResponse>().Data;

            try
            {
                await _cartItemRepository.DeleteManyAsync(x => x.CartId == cart.Id);

                return new OkResponse();
            }
            catch
            {
                return new ServerErrorResponse($"An error occurred while deleting the cart items from cart. Cart ID : {cart.Id}");
            }
        }

        public async Task<IResponse> UpdateCartItemAsync(int id, UpdateCartItemRequest request)
        {
            try
            {
                var result = await _cartItemRepository.UpdateAsync(id, entity =>
                {
                    entity.Quantity = request.Quantity;
                });

                if (!result)
                {
                    return new NotFoundResponse($"Cart item not found. ID : {id}");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse("An error occurred while updating the cart item.");
            }
        }
    }
}
