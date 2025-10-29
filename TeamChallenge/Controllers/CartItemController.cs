using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/cart-items")]
    [ApiController]
    [Authorize]
    public class CartItemController(ICartItemLogic cartItemLogic) : ControllerBase
    {
        [HttpPost]
        public async Task<IResponse> CreateCartItem([FromBody] CreateCartItemRequest request)
        {
            return await cartItemLogic.CreateCartItemAsync(request);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> UpdateCartItem([FromRoute]int id, [FromBody] UpdateCartItemRequest request)
        {
            return await cartItemLogic.UpdateCartItemAsync(id, request);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> DeleteCartItem([FromRoute] int id)
        {
            return await cartItemLogic.DeleteCartItemAsync(id);
        }
    }
}
