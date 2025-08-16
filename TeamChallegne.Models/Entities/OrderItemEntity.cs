using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class OrderItemEntity : BaseEntity
    {
        [ForeignKey("Order")]
        public required int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        [ForeignKey("Product")]
        public required int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
    }
}