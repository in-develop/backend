using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class CartItemEntity : BaseEntity
    {
        [ForeignKey("Cart")]
        [Required]
        public int CartId { get; set; }
        public CartEntity Cart { get; set; }
        [ForeignKey("Product")]
        [Required]
        public  int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
    }
}