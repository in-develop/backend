using TeamChallenge.Models.Entities;
namespace TeamChallenge.Repositories
{
    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<OrderEntity?> GetOrderWithDetailsAsync(int orderId);
    }
}
