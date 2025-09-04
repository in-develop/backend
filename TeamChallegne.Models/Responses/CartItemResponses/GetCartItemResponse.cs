using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CartItemResponses
{
    public class GetCartItemResponse : BaseDataResponse<CartItemEntity>
    {
        public GetCartItemResponse(CartItemEntity data) : base(data)
        {
        }
    }
}
