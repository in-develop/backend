using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class ResendEmailRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}
