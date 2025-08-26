using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetReviewResponse : BaseDataResponse<ReviewEntity>
    {
        public GetReviewResponse(ReviewEntity data) : base(data)
        {
        }
    }
}
