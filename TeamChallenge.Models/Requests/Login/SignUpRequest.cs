using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
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
