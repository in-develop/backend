using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        private readonly CosmeticStoreDbContext _context;
        public CategoryRepository(CosmeticStoreDbContext context, ILogger<CategoryRepository> logger) : base(context, logger)
        {
            _context = context;
        }

        protected override async Task<IEnumerable<CategoryEntity>> DoGetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .ToListAsync();
        }

        protected override async Task<CategoryEntity?> DoGetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}