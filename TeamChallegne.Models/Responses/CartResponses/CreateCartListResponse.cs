using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class CreateCartListResponse : DataListResponse<CartItemEntity>
    {
        public CreateCartListResponse(IEnumerable<CartItemEntity> data) : base(data)
        {
        }
    }
}
