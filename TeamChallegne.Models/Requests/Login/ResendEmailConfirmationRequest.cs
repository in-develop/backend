using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class ResendEmailConfirmationRequest
    {
        [Required]
        public string Email { get; set; }

    }
}
