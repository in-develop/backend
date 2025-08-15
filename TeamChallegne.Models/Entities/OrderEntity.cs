using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        [ForeignKey("User")]
        public required string UserId { get; set; }
        public UserEntity User { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}