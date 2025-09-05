using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetProductBundleResponse : BaseDataResponse<ProductBundleEntity>
{
    public GetProductBundleResponse(ProductBundleEntity data) : base(data)
    {
    }
}
