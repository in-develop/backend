using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CartResponses
{
    public class CreateCartListResponse : DataListResponse<CartItemEntity>
    {
        public CreateCartListResponse(IEnumerable<CartItemEntity> data) : base(data)
        {
        }
    }
}
