using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductLogic _productLogic;
        public ProductController(IProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productLogic.GetAllProductsAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _productLogic.GetProductByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest requestData)
        {
            var result = await _productLogic.CreateProductAsync(requestData);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateProductRequest requestData)
        {
            var result = await _productLogic.UpdateProductAsync(id, requestData);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result = await _productLogic.DeleteProductAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
