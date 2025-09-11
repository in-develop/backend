
using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.CartItem
{
    public class CreateCartItemRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
