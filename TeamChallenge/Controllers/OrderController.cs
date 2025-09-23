using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderLogic _orderLogic;
        public OrderController(IOrderLogic orderLogic)
        {
            _orderLogic = orderLogic;
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetOrder([FromRoute]int id)
        {
            return await _orderLogic.GetOrderAsync(id);
        }

        [HttpGet]
        public async Task<IResponse> CreateOrder()
        {
            return await _orderLogic.CreateOrderAsync();
        }

        [HttpGet("submit/{id}")]
        public async Task<IResponse> SubmitOrder([FromRoute] int id)
        {
            return await _orderLogic.SubmitOrderAsync(id);
        }

        [HttpDelete]
        public async Task<IResponse> DeleteOrder([FromRoute] int id)
        {
            return await _orderLogic.DeleteOrderAsync(id);
        }
    }
}
