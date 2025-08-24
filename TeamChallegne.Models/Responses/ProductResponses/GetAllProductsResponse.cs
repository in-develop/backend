using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetAllProductsResponse : BaseDataListResponse<ProductEntity>
    {
        public GetAllProductsResponse(IEnumerable<ProductEntity> data) : base(data)
        {
        }
    }
}
