using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(CosmeticStoreDbContext context) : base(context)
        {
        }
    }

    public class ReviewRepository : BaseRepository<ReviewEntity>, IReviewRepository
    {
        public ReviewRepository(CosmeticStoreDbContext context) : base(context)
        {
        }
    }
}