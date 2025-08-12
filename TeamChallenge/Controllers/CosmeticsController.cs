using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Models;
using TeamChallenge.Models.DTOs;
using TeamChallenge.Services;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/[Cosmetic]")]
    public class CosmeticController : ControllerBase
    {
        private readonly CosmeticService _service;

        public CosmeticController(CosmeticService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAllAsync()); // невпевнений
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }    
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cosmetic = await _service.GetByIdAsync(id);
                return cosmetic == null ? NotFound($"Id is not found({id})") : Ok(cosmetic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CosmeticCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CosmeticCreateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated ? Ok("Product is successfuly updated") : NotFound("Product is not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                return deleted ? NotFound("Product is not found.") : Ok("Product is successfuly deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
