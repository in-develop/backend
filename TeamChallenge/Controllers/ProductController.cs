using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Product;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Static_data;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IProductLogic productLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await productLogic.GetAllProductsAsync();

        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await productLogic.GetProductByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Create([FromBody] CreateProductRequest requestData)
        {
            return await productLogic.CreateProductAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateProductRequest requestData)
        {
            return await productLogic.UpdateProductAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await productLogic.DeleteProductAsync(id);
        }

        [HttpDelete("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> DeleteMany([FromBody] DeleteProductManyRequest requestData)
        {
            return await productLogic.DeleteManyProductsAsync(requestData.Ids);
        }
    }
}
