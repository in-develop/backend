using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class ProductGetByIdResponse : BaseDataResponse<ProductEntity>
    {
        public ProductGetByIdResponse(ProductEntity data) : base(data)
        {
        }
    }
}
