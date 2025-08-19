using TeamChallenge.Models.DTOs.Category;
using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Models.Models.Responses.CategoryResponse
{
    public class CategoryResponse : BaseDataResponse<CategoryEntity>
    {
        public CategoryResponse(CategoryEntity data) : base(data)
        {

        }
    }
}
