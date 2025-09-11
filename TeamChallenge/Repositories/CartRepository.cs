using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class CartRepository : BaseRepository<CartEntity>, ICartRepository
    {
        public CartRepository(CosmeticStoreDbContext context, ILogger<IRepository<CartEntity>> logger) : base(context, logger)
        {
        }

        public async Task<CartEntity?> GetCartByUserId(string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(item => item.UserId == userId);
        }

        public async Task<CartEntity?> GetCartWithCartItemsAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(ci => ci.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
