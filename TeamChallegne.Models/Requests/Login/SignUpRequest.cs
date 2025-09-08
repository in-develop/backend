using TeamChallenge.Models.DTOs;

namespace TeamChallenge.Models.Requests.Login
{
    public class SignUpRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
        public ICollection<CartItemDTO>? CartItems { get; set; }

    }
}
