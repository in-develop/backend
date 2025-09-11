using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Login
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
