using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests.Category;

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
        public async Task<IActionResult> GetAll()
        {
            var result = await _productLogic.GetAllCategoriesAsync();

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _productLogic.GetCategoryByIdAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest requestData)
        {
            var result = await _productLogic.CreateCategoryAsync(requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateCategoryRequest requestData)
        {
            var result = await _productLogic.UpdateCategoryAsync(id, requestData);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result = await _productLogic.DeleteCategoryAsync(id);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}