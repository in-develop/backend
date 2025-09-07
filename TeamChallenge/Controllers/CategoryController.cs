using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Category;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;

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
        [ProducesResponseType(typeof(GetCategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IResponse> GetById([FromRoute]int id)
        {
            return await _categoryLogic.GetCategoryByIdAsync(id);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateCategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IResponse> Create([FromBody] CreateCategoryRequest requestData)
        {
            return await _categoryLogic.CreateCategoryAsync(requestData);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OkResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResponse> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
            return await _categoryLogic.UpdateCategoryAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute]int id)
        {
            return await _categoryLogic.DeleteCategoryAsync(id);
        }
    }
}