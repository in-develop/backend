using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.SubCategory;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Static_data;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/subcategories")]
    public class SubCategoryController(ISubCategoryLogic subCategoryLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await subCategoryLogic.GetAllSubCategoryAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute] int id)
        {
            return await subCategoryLogic.GetSubCategoryByIdAsync(id);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Create([FromBody] CreateSubCategoryRequest requestData)
        {
            return await subCategoryLogic.CreateSubCategoryAsync(requestData);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> CreateMany([FromBody] List<CreateSubCategoryManyRequest> requestData)
        {
            return await subCategoryLogic.CreateSubCategoryManyAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateSubCategoryRequest requestData)
        {
            return await subCategoryLogic.UpdateSubCategoryAsync(id, requestData);
        }

        [HttpPut("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> UpdateMany([FromBody] List<UpdateSubCategoryManyRequest> requestData)
        {
            return await subCategoryLogic.UpdateSubCategoryManyAsync(requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await subCategoryLogic.DeleteSubCategoryAsync(id);
        }

        [HttpDelete("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> DeleteMany([FromBody] DeleteSubCategoryManyRequest requestData)
        {
            return await subCategoryLogic.DeleteSubCategoryManyAsync(requestData.Ids);
        }
    }
}
