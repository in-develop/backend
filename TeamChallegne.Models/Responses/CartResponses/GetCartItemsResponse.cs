using TeamChallenge.Models.DTOs.Cart;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CartResponses
{
    public class GetCartItemsResponse : BaseDataListResponse<GetCartItemsDTO>
    {
        public GetCartItemsResponse(IEnumerable<GetCartItemsDTO> data) : base(data)
        {
        }
    }
}
