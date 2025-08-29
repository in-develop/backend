using TeamChallenge.Models.DTOs.Cart;

namespace TeamChallenge.Models.Requests.Cart
{
    public class CreateCartRequest
    {
        public string UserId { get; set; } 
        public ICollection<CreateCartItemDTO> CartItems { get; set; }
    }
}
