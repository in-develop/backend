using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface ICartLogic
    {
        Task<IResponse> GetCartWithCartItems();
        Task<IResponse> DeleteCartAsync(int id);
        Task<IResponse> GetValidCart();
    }
}
