using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface IOrderLogic
    {
        Task<IResponse> GetOrderAsync(int orderId);
        Task<IResponse> CreateOrderAsync();
        Task<IResponse> SubmitOrderAsync(int orderId);
        Task<IResponse> DeleteOrderAsync(int orderId);
    }
}
