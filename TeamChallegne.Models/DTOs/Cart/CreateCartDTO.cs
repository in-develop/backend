
namespace TeamChallenge.Models.DTOs.Cart
{
    public class CreateCartItemDTO
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
