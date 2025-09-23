using Microsoft.EntityFrameworkCore;
using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class OrderHistoryRepository : BaseRepository<OrderHistoryEntity>, IOrderHistoryRepository
    {
        public OrderHistoryRepository(CosmeticStoreDbContext context, ILogger<OrderHistoryRepository> logger) : base(context, logger)
        {
        }

        public async Task<OrderHistoryEntity?> GetLatestOrderHistoryItem(int orderId)
        {
            return await _dbSet.Where(oh => oh.OrderId == orderId)
                         .OrderByDescending(oh => oh.ChangedAt)
                         .FirstOrDefaultAsync();
        }
    }
}