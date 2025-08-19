using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CategoryResponses
{
    public class CategoryGetAllResponse : BaseDataResponse<IEnumerable<CategoryEntity>>
    {
        public CategoryGetAllResponse(IEnumerable<CategoryEntity> data) : base(data)
        {
        }
    }
}
