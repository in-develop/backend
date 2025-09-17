using TeamChallenge.Models.Entities;

namespace TeamChallenge.Repositories
{
    public interface ICartRepository : IRepository<CartEntity>
    {
        Task<CartEntity?> GetCartByUserId(string UserId);
        Task<CartEntity?> GetCartWithCartItemsAsync(int id);

    }
}
