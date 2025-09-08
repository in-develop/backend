using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.DTOs;
using TeamChallenge.Models.DTOs.Cart;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;

namespace TeamChallenge.Repositories
{
    public class CartItemRepository : BaseRepository<CartItemEntity>, ICartItemRepository
    {
        public CartItemRepository(CosmeticStoreDbContext context, ILogger<IRepository<CartItemEntity>> logger) : base(context, logger)
        {
        }

        public async Task<bool> CreateCartItemAsync(List<CreateCartItemRequest> list, int cartId)
        {
            try
            {
                _logger.LogInformation("Creating new CartItems.");
                foreach (var item in list)
                {
                    if (await _dbSet.AnyAsync(x => x.ProductId == item.ProductId) ||
                        await _dbSet.AnyAsync(x => x.CartId == cartId))
                    {
                        await _dbSet.AddAsync(new CartItemEntity
                        {
                            CartId = cartId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                    }
                    else
                    {
                        _logger.LogError("Failed to create CartItem: ProductId {0} or CartId {1} does not exist.", item.ProductId, cartId);
                        return false;
                    }
                }

                await SaveChangesAsync();
                return true;
            }
            catch
            {
                _logger.LogError("An error occurred while creating CartItems.");
                return false;
            }
        }

        public async Task<IEnumerable<GetCartItemsDTO>> GetWithProductsAndCartAsync(int cartId)
        {
            var result = await _dbSet
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product)
                .Select(ci => new GetCartItemsDTO
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity
                })
                .ToListAsync();

            return result;
        }
    }
}
