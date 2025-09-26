using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{

    [Route("api/carts")]
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
        public async Task<IResponse> GetCart()
        {
            return await _cartLogic.GetCartWithCartItems();
        }
    }
}
