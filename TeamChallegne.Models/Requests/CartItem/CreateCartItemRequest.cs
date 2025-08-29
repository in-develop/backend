
namespace TeamChallenge.Models.Requests.CartItem
{
    public class CreateCartItemRequest
    {
        public Guid? UserId { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
    }
}
