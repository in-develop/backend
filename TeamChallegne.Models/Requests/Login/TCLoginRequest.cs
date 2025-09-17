using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class TCLoginRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
