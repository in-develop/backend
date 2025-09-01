using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.CartResponses
{
    public class GetСartResponse : DataResponse<CartEntity>
    {
        public GetСartResponse(CartEntity data) : base(data)
        {
        }
    }
}
