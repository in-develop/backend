using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Logic
{
    public interface IReviewLogic
    {
        Task<IResponse> GetAllReviewsAsync();
        Task<IResponse> GetReviewByIdAsync(int id);
        //Task<IResponse> GetSortedReviewAsync(int id);
        Task<IResponse> CreateReviewAsync(CreateReviewRequest dto);
        Task<IResponse> UpdateReviewAsync(int id, UpdateReviewRequest dto);
        Task<IResponse> DeleteReviewAsync(int id);
    }
}
