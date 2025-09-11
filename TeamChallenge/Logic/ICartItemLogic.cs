using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartItemLogic
    {
        Task<IResponse> CreateCartItemAsync(CreateCartItemRequest dto);
        Task<IResponse> CreateCartItemAsync(int cartId, List<CreateCartItemRequest> dto);
        Task<IResponse> UpdateCartItemAsync(int id, UpdateCartItemRequest dto);
        Task<IResponse> DeleteCartItemAsync(int id);
    }
}
