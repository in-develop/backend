using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;

namespace TeamChallenge.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartLogic _cartLogic;
        public CartController(ICartLogic cartLogic)
        {
            _cartLogic = cartLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartLogic.GetCartWithCartItems();
            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
