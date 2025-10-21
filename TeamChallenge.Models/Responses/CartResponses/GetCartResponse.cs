using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetCartResponse : BaseDataResponse<CartEntity>
    {
        public GetCartResponse(CartEntity data) : base(data)
        {
        }
    }
}
