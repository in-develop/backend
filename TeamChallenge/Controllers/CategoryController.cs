using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Models.Responses;
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

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productLogic.GetAllCategoriesAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _productLogic.GetCategoryByIdAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest requestData)
        {
            var result = await _productLogic.CreateCategoryAsync(requestData);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody] UpdateCategoryRequest requestData)
        {
            var result = await _productLogic.UpdateCategoryAsync(id, requestData);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var result = await _productLogic.DeleteCategoryAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}