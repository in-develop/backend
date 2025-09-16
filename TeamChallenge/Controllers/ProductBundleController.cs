using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.StaticData;

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
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Create([FromBody] CreateProductBundleRequest requestData)
        {
            return await _bundleLogic.CreateProductBundleAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateProductBundleRequest requestData)
        {
            return await _bundleLogic.UpdateProductBundleAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await _bundleLogic.DeleteProductBundleAsync(id);
        }
    }
}
