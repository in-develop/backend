using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Authorize]
    public class CartController(ICartLogic cartLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetCart()
        {
            return await cartLogic.GetCartWithCartItems();
        }
    }
}
