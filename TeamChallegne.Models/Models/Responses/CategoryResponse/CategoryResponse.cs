using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Models.Models.Responses
{
    public class CategoryResponse : BaseDataResponse<CategoryEntity>
    {
        public CategoryResponse(CategoryEntity data) : base(data)
        {

        }
    }
}
