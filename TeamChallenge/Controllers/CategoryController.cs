using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Models.Responses.CategoryResponse;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _service;



        public CategoryController(ICategory service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var category = await _service.GetAllAsync();
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
                var category = await _service.GetByIdAsync(id);
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
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CategoryAddDto dto)
        {
            try
            {
                var id = int.Parse((string)RouteData.Values["id"]);
                var updated = await _service.UpdateAsync(id, dto);
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
                var deleted = await _service.DeleteAsync(id);
                return deleted ? Ok(new OkResponse("Category is successfuly deleted")) : NotFound(new ErrorResponse("Desired Category is not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}