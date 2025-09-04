using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Requests.CartItem;

namespace TeamChallenge.Repositories
{
    public interface ICartItemRepository: IRepository<CartItemEntity>
    {
        Task<bool> CreateCartItemAsync(List<CreateCartItemRequest> list);
    }
}
