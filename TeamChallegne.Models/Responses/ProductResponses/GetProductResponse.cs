using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetProductResponse : BaseDataResponse<GetProductResponseModel>
{
    public GetProductResponse(GetProductResponseModel data) : base(data)
    {
    }
}
