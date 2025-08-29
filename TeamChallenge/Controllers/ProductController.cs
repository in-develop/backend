using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

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
        public async Task<IResponse> GetAll()
        {
            return await _productLogic.GetAllProductsAsync();

        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _productLogic.GetProductByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateProductRequest requestData)
        {
            return await _productLogic.CreateProductAsync(requestData);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateProductRequest requestData)
        {
            return await _productLogic.UpdateProductAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _productLogic.DeleteProductAsync(id);
        }
    }
}
