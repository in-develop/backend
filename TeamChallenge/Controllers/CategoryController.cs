using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Models.Responses;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.CategoryResponses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _service;

        public CategoryController(ICategoryLogic service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var category = await _service.GetAllCategoriesAsync();
                return Ok(new CategoryGetAllResponse(category));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById()
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var category = await _service.GetCategoryByIdAsync(id);
                return category == null ? NotFound(new ErrorResponse($"Your category {id} is not found")) : Ok(new CategoryResponse(category));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            try
            {
                var data = await _service.CreateCategoryAsync(dto);

                if (data != null) return Ok(new CategoryCreateResponse(data));
                return NotFound(new ErrorResponse(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateDto dto)
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var updated = await _service.UpdateCategoryAsync(id, dto);
                return updated ? Ok(new OkResponse("Category is successfuly updated")) : NotFound(new ErrorResponse("Desired Category is not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var deleted = await _service.DeleteCategoryAsync(id);
                return deleted ? Ok(new OkResponse("Category is successfuly deleted")) : NotFound(new ErrorResponse("Desired Category is not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}