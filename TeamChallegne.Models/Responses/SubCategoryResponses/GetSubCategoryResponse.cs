using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.SubCategoryResponses
{
    public class GetSubCategoryResponse : BaseDataResponse<SubCategoryEntity>
    {
        public GetSubCategoryResponse(SubCategoryEntity data) : base(data)
        {
        }
    }
}
