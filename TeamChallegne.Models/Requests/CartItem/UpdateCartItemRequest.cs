
using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.CartItem
{
    public class UpdateCartItemRequest
    {
        [Required]
        public int Quantity { get; set; }
    }
}
