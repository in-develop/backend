using System.ComponentModel.DataAnnotations;

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
    }
}
