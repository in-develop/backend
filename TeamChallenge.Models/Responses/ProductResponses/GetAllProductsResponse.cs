using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses.ProductResponses;

namespace TeamChallenge.Models.Responses;

public class GetAllProductsResponse : BaseDataListResponse<ProductSummaryResponseModel>
{
    public GetAllProductsResponse(IEnumerable<ProductSummaryResponseModel> data) : base(data)
    {
    }
}
