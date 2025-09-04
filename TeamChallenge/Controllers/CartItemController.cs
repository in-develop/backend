using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.DTOs;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] int id)
        {
            var result = await _cartItemLogic.GetCartItemAsync(id);

            // This is old method to send responses, in master branch it handled by actionFilter so you only need to return result
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
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

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCartItem([FromQuery] int id, [FromBody] UpdateCartItemRequest request)
        {
            var result = await _cartItemLogic.UpdateCartItemAsync(id, request);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCartItem([FromQuery] int id)
        {
            var result = await _cartItemLogic.DeleteCartItemAsync(id);
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
