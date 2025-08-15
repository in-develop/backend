using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Models.Models.Responses.CategoryResponse
{
    public class CategoryResponse : BaseDataResponse<CategoryReadDto>
    {
        public CategoryResponse(CategoryReadDto data) : base(data)
        {

        }
    }
}
