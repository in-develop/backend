using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.CartItem;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateCartItem([FromBody] CreateCartItemRequest request)
        {
            var result = await _cartItemLogic.CreateCartItemAsync(request);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute]int id, [FromBody] UpdateCartItemRequest request)
        {
            var result = await _cartItemLogic.UpdateCartItemAsync(id, request);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int id)
        {
            var result = await _cartItemLogic.DeleteCartItemAsync(id);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
