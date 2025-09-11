using System.ComponentModel.DataAnnotations;
using TeamChallenge.Models.Requests.CartItem;

namespace TeamChallenge.Models.Requests.Login
{
    public class SignUpRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }        
        public List<CreateCartItemRequest>? CartItems { get; set; }

    }
}
