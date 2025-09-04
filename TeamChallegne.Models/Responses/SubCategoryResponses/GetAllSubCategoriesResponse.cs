using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.SubCategoryResponses
{
    public class GetAllSubCategoriesResponse : BaseDataListResponse<SubCategoryEntity>
    {
        public GetAllSubCategoriesResponse(IEnumerable<SubCategoryEntity> data) : base(data)
        {
        }
    }
}
