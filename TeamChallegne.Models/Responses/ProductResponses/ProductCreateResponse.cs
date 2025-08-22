using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.ProductResponses
{
    public class ProductCreateResponse : BaseDataResponse<ProductEntity>
    {
        public ProductCreateResponse(ProductEntity data) : base(data)
        {
        }
    }
}
