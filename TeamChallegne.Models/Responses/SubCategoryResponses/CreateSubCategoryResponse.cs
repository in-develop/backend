using TeamChallenge.Models.Models.Responses.SubCategoryResponse;

namespace TeamChallenge.Models.Responses.SubCategoryResponses
{
    public class CreateSubCategoryResponse : BaseDataResponse<SubCategoryResponse>
    {
        public CreateSubCategoryResponse(SubCategoryResponse data) : base(data)
        {
            StatusCode = 201;
        }
    }
}
