using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses.CategoryResponse;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var category = await _service.GetAllCategoriesAsync();
                return Ok(new CategoryListResponse(category));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            try
            {
                var data = await _service.CreateCategoryAsync(dto);

                if (data != null) return Ok(new CategoryResponse(data));
                return NotFound(new ErrorResponse(""));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }
    }
}