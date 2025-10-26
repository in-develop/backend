using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Static_data;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(ICategoryLogic categoryLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await categoryLogic.GetAllCategoriesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await categoryLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await categoryLogic.CreateCategoryAsync(requestData);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> CreateMany([FromBody] List<CreateCategoryManyRequest> requestData)
        {
            return await categoryLogic.CreateCategoryManyAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
            return await categoryLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpPut("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> UpdateMany([FromBody] List<UpdateCategoryManyRequest> requestData)
        {
            return await categoryLogic.UpdateCategoryManyAsync(requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await categoryLogic.DeleteCategoryAsync(id);
        }

        [HttpDelete("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<IResponse> DeleteMany([FromBody] DeleteCategoryManyRequest requestData)
        {
            return await categoryLogic.DeleteCategoryManyAsync(requestData.Ids);
        }
    }
}