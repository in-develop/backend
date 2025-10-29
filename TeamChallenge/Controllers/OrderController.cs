using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrderLogic orderLogic) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IResponse> GetOrder([FromRoute]int id)
        {
            return await orderLogic.GetOrderAsync(id);
        }

        [HttpGet]
        public async Task<IResponse> CreateOrder()
        {
            return await orderLogic.CreateOrderAsync();
        }

        [HttpGet("submit/{id}")]
        public async Task<IResponse> SubmitOrder([FromRoute] int id)
        {
            return await orderLogic.SubmitOrderAsync(id);
        }

        [HttpDelete]
        public async Task<IResponse> DeleteOrder([FromRoute] int id)
        {
            return await orderLogic.DeleteOrderAsync(id);
        }
    }
}
