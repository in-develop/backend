using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Models.Models.Responses
{
    public class CategoryCreateResponse : BaseDataResponse<CategoryEntity>
    {
        public CategoryCreateResponse(CategoryEntity data) : base(data)
        {

        }
    }
}
