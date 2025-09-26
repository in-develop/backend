using TeamChallenge.DbContext;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(CosmeticStoreDbContext context, ILogger<OrderItemRepository> logger) : base(context, logger)
        {
        }
    }
}