using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses.SubCategoryResponse;

namespace TeamChallenge.Models.Responses.SubCategoryResponses
{
    public class GetSubCategoryResponse : BaseDataResponse<SubCategoryResponse>
    {
        public GetSubCategoryResponse(SubCategoryResponse data) : base(data)
        {
            Message = $"successfully obtained SubCategory with Id = {data.Id}";
        }
    }
}
