using TeamChallenge.Helpers;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;
using TeamChallenge.Repositories;
using TeamChallenge.Services;

namespace TeamChallenge.Logic
{
    public class CartLogic : ICartLogic
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartLogic> _logger;
        private readonly ITokenReaderService _tokenReader;

        public CartLogic(
            RepositoryFactory factory, 
            ILogger<CartLogic> logger,
            ITokenReaderService tokenReader)
        {
            _cartRepository = (ICartRepository)factory.GetRepository<CartEntity>();
            _logger = logger;
            _tokenReader = tokenReader;
        }

        public async Task<IResponse> DeleteCartAsync()
        {
            var response = await GetCart();

            if (!response.IsSuccess)
            {
                return response;
            }

            var cartId = response.As<GetCartResponse>().Data.Id;

            try
            {
                var result = await _cartRepository.DeleteAsync(cartId);
                return new OkResponse();
            }
            catch (Exception ex)
            {
                return new ServerErrorResponse($"Fail to delete cart. ID : {cartId}");
            }
        }

        public async Task<IResponse> GetCartWithCartItems()
        {
            var response = await GetCart();

            if (!response.IsSuccess)
            {
                return response;
            }

            var cartId = response.As<GetCartResponse>().Data.Id;

            try
            {
                var cart = await _cartRepository.GetCartWithCartItemsAsync(cartId);

                return new GetCartItemsResponse(cart.CartItems.Select(x => new GetCartItemsResponseModel(x)));
            }
            catch
            {
                return new ServerErrorResponse($"Fail to get cart. ID : {cartId}");
            }
        }

        public async Task<IResponse> GetCart()
        {
            var response = _tokenReader.GetCartId();

            if (!response.IsSuccess)
            {
                return response;
            }

            var cartId = response.As<GetCartIdResponse>().Data;
            var cart = await _cartRepository.GetByIdAsync(cartId);

            if (cart == null)
            {
                _logger.LogError("Cart not found. ID: {0}", cartId);
                return new NotFoundResponse($"Cart not found. ID: {cartId}");
            }

            return new GetCartResponse(cart);
        }
    }
}
