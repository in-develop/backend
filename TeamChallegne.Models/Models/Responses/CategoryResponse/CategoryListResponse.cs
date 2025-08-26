using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;


namespace TeamChallenge.Models.Models.Responses
{
    public class CategoryListResponse : BaseDataListResponse<CategoryEntity>
    {
        public CategoryListResponse(IEnumerable<CategoryEntity> data) : base(data)
        {
        }
    }
}
