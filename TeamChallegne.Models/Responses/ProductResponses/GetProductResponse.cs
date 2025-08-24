using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetProductResponse : BaseDataResponse<ProductEntity>
    {
        public GetProductResponse(ProductEntity data) : base(data)
        {
        }
    }
}
