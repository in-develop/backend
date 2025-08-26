using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductLogic _productLogic;
        public ProductController(IProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productLogic.GetAllProductsAsync();

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _productLogic.GetProductByIdAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest requestData)
        {
            var result = await _productLogic.CreateProductAsync(requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateProductRequest requestData)
        {
            var result = await _productLogic.UpdateProductAsync(id, requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result = await _productLogic.DeleteProductAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
