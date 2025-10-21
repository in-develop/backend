
using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class UpdateCartItemRequest
    {
        [Required]
        public int Quantity { get; set; }
    }
}
