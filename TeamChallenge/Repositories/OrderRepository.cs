using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(CosmeticStoreDbContext context, ILogger<OrderRepository> logger) : base(context, logger)
        {
        }

        public async Task<OrderEntity?> GetOrderWithDetailsAsync(int id)
        {
            _logger.LogInformation("Fetching order with ID = {0}", id);
            var result = await _dbSet
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                _logger.LogWarning("Order with ID = {1} not found.", id);
            }

            return result;
        }
    }
}