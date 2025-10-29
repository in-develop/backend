using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Static_data;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/bundles")]
    public class ProductBundleController(IProductBundleLogic productLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await productLogic.GetAllProductBundlesAsync();

        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute] int id)
        {
            return await productLogic.GetProductBundleByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Create([FromBody] CreateProductBundleRequest requestData)
        {
            return await productLogic.CreateProductBundleAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateProductBundleRequest requestData)
        {
            return await productLogic.UpdateProductBundleAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await productLogic.DeleteProductBundleAsync(id);
        }
    }
}
