using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;
using TeamChallenge.StaticData;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _categoryLogic;

        public CategoryController(ICategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await _categoryLogic.GetAllCategoriesAsync();
        }

        [HttpGet("{id}")]

        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _categoryLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await _categoryLogic.CreateCategoryAsync(requestData);
        }

        [HttpPost("bulk")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> CreateMany([FromBody] List<CreateCategoryManyRequest> requestData)
        {
            return await _categoryLogic.CreateCategoryManyAsync(requestData);
        }

        [HttpPut("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
            return await _categoryLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpPut("bulk")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> UpdateMany([FromBody] List<UpdateCategoryManyRequest> requestData)
        {
            return await _categoryLogic.UpdateCategoryManyAsync(requestData);
        }

        [HttpDelete("{id}")]
        [Authorize(GlobalConsts.Roles.Admin)]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _categoryLogic.DeleteCategoryAsync(id);
        }
    }
}