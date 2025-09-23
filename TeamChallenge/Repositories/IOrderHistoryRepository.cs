using TeamChallenge.Models.Entities;
namespace TeamChallenge.Repositories
{
    public interface IOrderHistoryRepository : IRepository<OrderHistoryEntity>
    {
        Task<OrderHistoryEntity?> GetLatestOrderHistoryItem(int orderId);
    }
}
