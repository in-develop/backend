using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Models.Models.Responses.CategoryResponse
{
    public class CategoryCreateResponse : BaseDataResponse<CategoryEntity>
    {
        public CategoryCreateResponse(CategoryEntity data) : base(data)
        {

        }
    }
}
