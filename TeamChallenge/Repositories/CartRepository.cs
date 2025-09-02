using TeamChallenge.DbContext;
using TeamChallenge.Models.DTOs.Cart;
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

        public async Task<bool> CreateCartItemsBulk(ICollection<CreateCartItemDTO> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                return false;
            }

            foreach (var item in cartItems)
            {
                var cartItemEntity = new CartItemEntity
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CartId = item.CartId
                };
            }

            await SaveChangesAsync();
            return true;
        }
    }
}
