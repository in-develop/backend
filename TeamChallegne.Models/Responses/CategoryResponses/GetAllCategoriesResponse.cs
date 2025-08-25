using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetAllCategoriesResponse : BaseDataListResponse<CategoryEntity>
    {
        public GetAllCategoriesResponse(IEnumerable<CategoryEntity> data) : base(data)
        {
        }
    }
}
