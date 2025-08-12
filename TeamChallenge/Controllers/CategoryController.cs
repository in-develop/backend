using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Models;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Route("api/[Category]")]
    public class CategoryController : ControllerBase
    {
        private readonly CosmeticStoreDbContext _context;

        public CategoryController(CosmeticStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _context.Category.ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}