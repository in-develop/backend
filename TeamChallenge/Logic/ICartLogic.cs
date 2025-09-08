using TeamChallenge.Models.Requests.Cart;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartLogic
    {
        Task<IResponse> GetCart();
        Task<IResponse> DeleteCartAsync(int id);
    }
}
