using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.DTOs.Category;

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
                return Ok(await _service.GetAllAsync());
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
                return category == null ? NotFound($"Your category {id} is not found") : Ok(category);
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
                return updated ? Ok("Category is successfuly updated") : NotFound("Desired Category is not found");
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
                return deleted ? Ok("Category is successfuly deleted") : NotFound("Desired Category is not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}