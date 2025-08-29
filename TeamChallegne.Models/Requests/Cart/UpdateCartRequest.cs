using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Requests.Cart
{
    public class UpdateCartRequest
    {
        public ICollection<CartItemEntity> CartItems { get; set; }
    }
}
