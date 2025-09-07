using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(CosmeticStoreDbContext context, ILogger<CategoryRepository> logger) : base(context, logger)
        {
        }

        protected override async Task<IEnumerable<CategoryEntity>> DoGetAllAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<CategoryEntity?> GetByIdWithSubCategoriesAsync(int id)
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}