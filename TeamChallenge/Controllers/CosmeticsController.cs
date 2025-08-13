using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Models.DTOs;
using TeamChallenge.Services;
using TeamChallenge.Interfaces;


namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CosmeticController : ControllerBase
    {
        private readonly ICosmetic _service;

        public CosmeticController(ICosmetic service)
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cosmetic = await _service.GetByIdAsync(id);
                return cosmetic == null ? NotFound($"Your cosmetic {id} is not found") : Ok(cosmetic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CosmeticCreateDto dto)
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
        public async Task<IActionResult> Update(int id, [FromBody] CosmeticCreateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated ? Ok("Product is successfuly updated") : NotFound("Product is not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                return deleted ? Ok("Product is successfuly deleted") : NotFound("Product is not found.") ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
