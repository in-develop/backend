using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Login
{
    public class ResendEmailRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}
