using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Models.Responses;

namespace TeamChallenge.Models.Responses
{
    public class GetCategoryResponse : BaseDataResponse<CategoryResponse>
    {
        public GetCategoryResponse(CategoryResponse data) : base(data)
        {
            
        }
    }
}
