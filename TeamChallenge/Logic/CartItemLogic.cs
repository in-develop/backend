using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;

namespace TeamChallenge.Logic
{
    public class CartItemLogic : ICartItemLogic
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartLogic _cartLogic;
        private readonly ILogger<CartItemLogic> _logger;

        public CartItemLogic(
            RepositoryFactory factory, 
            ILogger<CartItemLogic> logger, 
            ICartLogic cartLogic)
        {
            _cartItemRepository = (ICartItemRepository)factory.GetRepository<CartItemEntity>();
            _productRepository = (IProductRepository)factory.GetRepository<ProductEntity>();
            _logger = logger;
            _cartLogic = cartLogic;
        }

        private async Task<IResponse> CheckIfProductsExists(params int[] productIds)
        {
            var existingProducts = await _productRepository.GetFilteredAsync(p => productIds.Contains(p.Id));
            var existingProductIds = existingProducts.Select(p => p.Id).ToHashSet();
            var missingProductIds = productIds.Except(existingProductIds).ToList();

            if (missingProductIds.Any())
            {
                _logger.LogWarning("Products not found: {MissingProductIds}", string.Join(", ", missingProductIds));
                return new NotFoundResponse($"Products not found: {string.Join(", ", missingProductIds)}");
            }

            return new OkResponse();
        }

        public async Task<IResponse> CreateCartItemAsync(CreateCartItemRequest request)
        {
            try
            {   
                var response = await CheckIfProductsExists(request.ProductId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                response = await _cartLogic.GetValidCart();

                if (!response.IsSuccess)
                {
                    return response;
                }

                var cart = (response as GetCartResponse).Data;

                await _cartItemRepository.CreateAsync(entity =>
                {
                    entity.ProductId = request.ProductId;
                    entity.Quantity = request.Quantity;
                    entity.CartId = cart.Id;
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse("An error occurred while creating the cart item.");
            }
        }

        public async Task<IResponse> CreateCartItemAsync(int cartId, List<CreateCartItemRequest> request)
        {
            try
            {
                var response = await CheckIfProductsExists(request.Select(x => x.ProductId).ToArray());

                if (!response.IsSuccess)
                {
                    return response;
                }

                await _cartItemRepository.CreateManyAsync(request.Count ,cartItems =>
                {
                    for (int i = 0; i < request.Count; i++)
                    {
                        cartItems[i].ProductId = request[i].ProductId;
                        cartItems[i].Quantity = request[i].Quantity;
                        cartItems[i].CartId = cartId;
                    }
                });

                return new OkResponse();
            }
            catch (Exception ex)
            {
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
                    return new NotFoundResponse($"Cart item with Id = {id} not found");
                }

                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse("An error occurred while deleting the cart item.");
            }
        }

        public async Task<IResponse> UpdateCartItemAsync(int id, UpdateCartItemRequest request)
        {
            try
            {

                var response = await _cartLogic.GetValidCart();

                if (!response.IsSuccess)
                {
                    return response;
                }

                var cart = (response as GetCartResponse).Data;

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
