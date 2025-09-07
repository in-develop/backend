using TeamChallenge.Models.Models.Responses;

namespace TeamChallenge.Models.Responses.CategoryResponses
{
    public class CreateCategoryResponse : BaseDataResponse<CategoryResponse>
    {
        public CreateCategoryResponse(CategoryResponse data) : base(data)
        {
            StatusCode = 201;
        }
    }
}
