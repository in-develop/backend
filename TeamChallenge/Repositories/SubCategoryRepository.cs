using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class SubCategoryRepository : BaseRepository<SubCategoryEntity>, ISubCategoryRepository
    {
        public SubCategoryRepository(CosmeticStoreDbContext context, ILogger<SubCategoryRepository> logger) : base(context, logger)
        {
        }
    }
}
