using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class SubCategoryRepository : BaseRepository<SubCategoryEntity>, ISubCategoryRepository
    {
        public SubCategoryRepository(CosmeticStoreDbContext context, ILogger<SubCategoryRepository> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<SubCategoryEntity>> GetAllWithCategoryAsync()
        {
            return await _dbSet
                .Include(sc => sc.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SubCategoryEntity?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(sc => sc.Category)
                .Include(sc => sc.ProductSubCategories)
                .ThenInclude(psc => psc.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }
    }
}
