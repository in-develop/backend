using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CartRepository: BaseRepository<CartEntity>, ICartRepository
    {
        public CartRepository(CosmeticStoreDbContext context, ILogger<IRepository<CartEntity>> logger) : base(context, logger)
        {
        }

        public async Task<bool> IsCartExist(string UserId)
        {
            var carts = await GetFilteredAsync(c => c.UserId == UserId);
            return carts.Any();
        }
    }
}
