using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/cart-items")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemLogic _cartItemLogic;
        public CartItemController(ICartItemLogic cartItemLogic)
        {
            _cartItemLogic = cartItemLogic;
        }

        [HttpPost]
        public async Task<IResponse> CreateCartItem([FromBody] CreateCartItemRequest request)
        {
            return await _cartItemLogic.CreateCartItemAsync(request);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> UpdateCartItem([FromRoute]int id, [FromBody] UpdateCartItemRequest request)
        {
            return await _cartItemLogic.UpdateCartItemAsync(id, request);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> DeleteCartItem([FromRoute] int id)
        {
            return await _cartItemLogic.DeleteCartItemAsync(id);
        }
    }
}
