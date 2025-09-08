using TeamChallenge.Models.DTOs;

namespace TeamChallenge.Models.Requests.Cart
{
    public class CreateCartRequest
    {
        public string UserId { get; set; } 
        public ICollection<CartItemDTO> CartItems { get; set; }
    }
}
