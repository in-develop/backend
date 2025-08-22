using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class ProductCreateResponse : BaseDataResponse<ProductEntity>
    {
        public ProductCreateResponse(ProductEntity data) : base(data)
        {
        }
    }
}
