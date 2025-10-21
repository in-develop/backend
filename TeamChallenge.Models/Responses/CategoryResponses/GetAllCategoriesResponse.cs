using TeamChallenge.Models.Models.Responses;

namespace TeamChallenge.Models.Responses
{
    public class GetAllCategoriesResponse : BaseDataListResponse<CategoryResponse>
    {
        public GetAllCategoriesResponse(IEnumerable<CategoryResponse> data) : base(data)
        {
        }
    }
}
