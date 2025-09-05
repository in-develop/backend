using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetAllProductBundlesResponse : BaseDataListResponse<ProductBundleEntity>
{
    public GetAllProductBundlesResponse(IEnumerable<ProductBundleEntity> data) : base(data)
    {
    }
}
