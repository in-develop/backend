using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Models.Models.Responses.CategoryResponse
{
    public class CategoryListResponse : BaseDataListResponse<CategoryReadDto>
    {
        public CategoryListResponse(IEnumerable<CategoryReadDto> data) : base(data)
        {
        }
    }
}
