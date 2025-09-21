using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;
using TeamChallenge.StaticData;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/subcategories")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryLogic _subCategoryLogic;

        public SubCategoryController(ISubCategoryLogic subCategoryLogic)
        {
            _subCategoryLogic = subCategoryLogic;
        }

        [HttpGet]
         public async Task<IResponse> GetAll()
        {
            return await _subCategoryLogic.GetAllSubCategoryAsync();
        }

        [HttpGet("{id}")]
         public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _subCategoryLogic.GetSubCategoryByIdAsync(id);
        }

        [HttpPost("create")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Create([FromBody] CreateSubCategoryRequest requestData)
        {
            return await _subCategoryLogic.CreateSubCategoryAsync(requestData);
        }

        [HttpPost("bulk")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> CreateMany([FromBody] List<CreateSubCategoryManyRequest> requestData)
        {
            return await _subCategoryLogic.CreateSubCategoryManyAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateSubCategoryRequest requestData)
        {
            return await _subCategoryLogic.UpdateSubCategoryAsync(id, requestData);
        }

        [HttpPut("bulk")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> UpdateMany([FromBody] List<UpdateSubCategoryManyRequest> requestData)
        {
            return await _subCategoryLogic.UpdateSubCategoryManyAsync(requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _subCategoryLogic.DeleteSubCategoryAsync(id);
        }

        [HttpDelete("bulk")]
        // [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> DeleteMany([FromBody] DeleteSubCategoryManyRequest requestData)
        {
            return await _subCategoryLogic.DeleteSubCategoryManyAsync(requestData.Ids);
        }
    }
}
