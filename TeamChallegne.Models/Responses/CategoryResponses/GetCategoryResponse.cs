using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CategoryResponses
{
    public class GetCategoryResponse : BaseDataResponse<CategoryEntity>
    {
        public GetCategoryResponse(CategoryEntity data) : base(data)
        {
            
        }
    }
}
