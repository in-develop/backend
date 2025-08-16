using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class CartItemEntity : BaseEntity
    {
        [ForeignKey("Cart")]
        public required int CartId { get; set; }
        public CartEntity Cart { get; set; }
        [ForeignKey("Product")]
        public required int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
    }
}