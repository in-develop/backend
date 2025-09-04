using TeamChallenge.Models.DTOs.Cart;
using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartLogic
    {
        Task<IResponse> GetCartByIdAsync(int id);
        Task<IResponse> CreateCartAsync(CreateCartRequest dto);
        Task<IResponse> DeleteCartAsync(int id);
        Task<IResponse> GetCartByUserIdAsync(string UserId);
    }
}
