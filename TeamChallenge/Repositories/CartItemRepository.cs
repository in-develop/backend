using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CartItemRepository : BaseRepository<CartItemEntity>, ICartItemRepository
    {
        public CartItemRepository(CosmeticStoreDbContext context, ILogger<IRepository<CartItemEntity>> logger) : base(context, logger)
        {
        }

    }
}
