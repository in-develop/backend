using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetAllReviewsResponse : BaseDataListResponse<ReviewEntity>
    {
        public GetAllReviewsResponse(IEnumerable<ReviewEntity> data) : base(data)
        {
        }
    }
}
