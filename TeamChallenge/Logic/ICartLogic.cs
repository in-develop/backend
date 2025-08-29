using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartLogic
    {
        Task<IResponse> GetByIdCartAsync(int id);
        Task<IResponse> CreateCartAsync(CreateCartRequest dto);
        Task<IResponse> UpdateCartAsync(int id, UpdateCartRequest dto);
        Task<IResponse> DeleteCartAsync(int id);
    }
}
