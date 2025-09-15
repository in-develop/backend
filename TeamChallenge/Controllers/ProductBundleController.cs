using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/bundles")]
    public class ProductBundleController : ControllerBase
    {
        private readonly IProductBundleLogic _bundleLogic;
        public ProductBundleController(IProductBundleLogic productLogic)
        {
            _bundleLogic = productLogic;
        }

        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await _bundleLogic.GetAllProductBundlesAsync();

        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute] int id)
        {
            return await _bundleLogic.GetProductBundleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateProductBundleRequest requestData)
        {
            return await _bundleLogic.CreateProductBundleAsync(requestData);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateProductBundleRequest requestData)
        {
            return await _bundleLogic.UpdateProductBundleAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await _bundleLogic.DeleteProductBundleAsync(id);
        }
    }
}
