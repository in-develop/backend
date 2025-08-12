using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Models;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api")]
    public class CosmeticController : ControllerBase
    {
        private readonly CosmeticStoreDbContext _context;

        public CosmeticController(CosmeticStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var cosmetics = await _context.Cosmetiс.ToListAsync();
            return Ok(cosmetics);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cosmetic = await _context.Cosmetiс.FindAsync(id);
            if (cosmetic == null)
            {
                return NotFound($"Id is not found({id})");
            }
            return Ok(cosmetic);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cosmetic cosmetic)
        {
            try
            {
                _context.Cosmetiс.Add(cosmetic);
                await _context.SaveChangesAsync();
                return Ok(new Response
                {
                    IsSucceded = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cosmetic cosmetic)
        {
            if (id != cosmetic.Id) 
            { 
                return NotFound("product is not found"); 
            }

            _context.Entry(cosmetic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cosmetic = await _context.Cosmetiс.FindAsync(id);
            if (cosmetic == null) return NotFound();

            _context.Cosmetiс.Remove(cosmetic);
            await _context.SaveChangesAsync();
            return Ok("");
        }
    }
}
