using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CategoryResponses
{
    public class CategoryGetAllResponse : BaseDataListResponse<CategoryEntity>
    {
        public CategoryGetAllResponse(IEnumerable<CategoryEntity> data) : base(data)
        {
        }
    }
}
