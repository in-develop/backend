using TeamChallenge.Models.DTOs;
using TeamChallenge.Models.DTOs.Cart;

namespace TeamChallenge.Models.Requests.Cart
{
    public class CreateCartRequest
    {
        public string UserId { get; set; } 
        public ICollection<CartItemDTO> CartItems { get; set; }
    }
}
