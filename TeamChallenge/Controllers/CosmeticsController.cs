using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Models;


namespace TeamChallenge.Controllers
{
    public class CosmeticsController
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CosmeticController : ControllerBase
        {
            private readonly CosmeticStoreDbContext _context;

            public CosmeticController(CosmeticStoreDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var cosmetics = await _context.Cosmetiс.ToListAsync();
                return Ok(cosmetics);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var cosmetic = await _context.Cosmetiс.FindAsync(id);
                if (cosmetic == null) return NotFound();
                return Ok(cosmetic);
            }
            
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] Cosmetic cosmetic)
            {
                _context.Cosmetiс.Add(cosmetic);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id  = cosmetic.Id }, cosmetic);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] Cosmetic cosmetic)
            {
                if (id != cosmetic.Id) return NotFound();

                _context.Entry(cosmetic).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var cosmetic = await _context.Cosmetiс.FindAsync(id);
                if (cosmetic == null) return NotFound();

                _context.Cosmetiс.Remove(cosmetic);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
