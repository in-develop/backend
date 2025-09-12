using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _productLogic;

        public CategoryController(ICategoryLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await _productLogic.GetAllCategoriesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _productLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await _productLogic.CreateCategoryAsync(requestData);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
            return await _productLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _productLogic.DeleteCategoryAsync(id);
        }
    }
}