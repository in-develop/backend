using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetAllProductBundlesResponse : BaseDataListResponse<GetProductBundleResponseModel>
{
    public GetAllProductBundlesResponse(IEnumerable<GetProductBundleResponseModel> data) : base(data)
    {
    }
}
