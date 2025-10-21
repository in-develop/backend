using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses.SubCategoryResponse;

namespace TeamChallenge.Models.Responses.SubCategoryResponses
{
    public class GetAllSubCategoriesResponse : BaseDataListResponse<SubCategoryResponse>
    {
        public GetAllSubCategoriesResponse(IEnumerable<SubCategoryResponse> data) : base(data)
        {
        }
    }
}
