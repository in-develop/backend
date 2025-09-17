using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses;

public class GetAllProductsResponse : BaseDataListResponse<GetProductResponseModel>
{
    public GetAllProductsResponse(IEnumerable<GetProductResponseModel> data) : base(data)
    {
    }
}
