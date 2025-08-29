using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Cart;

namespace TeamChallenge.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartLogic _cartLogic;
        public CartController(ICartLogic cartLogic)
        {
            _cartLogic = cartLogic;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request)
        {
            var result = await _cartLogic.CreateCartAsync(request);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart([FromRoute]int id, [FromBody]UpdateCartRequest request)
        {
            var result = await _cartLogic.UpdateCartAsync(id, request);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteCart([FromBody] int id)
        {
            var result = await _cartLogic.DeleteCartAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
