using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetProductBundleResponse : BaseDataResponse<GetProductBundleResponseModel>
{
    public GetProductBundleResponse(GetProductBundleResponseModel data) : base(data)
    {
    }
}
