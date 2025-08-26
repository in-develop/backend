using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class ReviewRepository : BaseRepository<ReviewEntity>, IReviewRepository
    {
        public ReviewRepository(CosmeticStoreDbContext context, ILogger<ReviewRepository> logger) : base(context, logger)
        {
        }
    }
}