using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Repositories
{
    public class CartItemRepository : BaseRepository<CartItemEntity>, ICartItemRepository
    {
        public CartItemRepository(CosmeticStoreDbContext context, ILogger<IRepository<CartItemEntity>> logger) : base(context, logger)
        {
        }
        public async Task<bool> CreateCartItemAsync(List<CreateCartItemRequest> list)
        {
            try
            {
                foreach (var item in list)
                {
                    await _dbSet.AddAsync(new CartItemEntity
                    {
                        CartId = item.CartId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                await SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
