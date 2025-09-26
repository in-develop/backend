using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Requests.Product;
using TeamChallenge.Models.Responses;
using TeamChallenge.StaticData;

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
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Create([FromBody] CreateProductRequest requestData)
        {
            return await _productLogic.CreateProductAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateProductRequest requestData)
        {
            return await _productLogic.UpdateProductAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        // [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _productLogic.DeleteProductAsync(id);
        }

        [HttpDelete("bulk")]
        public async Task<IResponse> DeleteMany([FromBody] DeleteProductManyRequest requestData)
        {
            return await _productLogic.DeleteManyProductsAsync(requestData.Ids);
        }
    }
}
