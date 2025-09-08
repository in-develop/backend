using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Requests.CartItem;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartItemLogic
    {
        Task<IResponse> CreateCartItemAsync(CreateCartItemRequest dto);
        Task<IResponse> CreateCartItemAsync(List<CreateCartItemRequest> list);
        Task<IResponse> UpdateCartItemAsync(UpdateCartItemRequest dto);
        Task<IResponse> DeleteCartItemAsync(int id);
        Task<IResponse> GetCartItemAsync(int id);
    }
}
