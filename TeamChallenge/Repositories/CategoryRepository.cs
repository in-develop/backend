using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(CosmeticStoreDbContext context, ILogger<CategoryRepository> logger) : base(context, logger)
        {
        }
    }
}