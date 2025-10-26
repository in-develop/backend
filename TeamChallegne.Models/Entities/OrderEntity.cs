using System.ComponentModel.DataAnnotations.Schema;
using TeamChallenge.StaticData;

namespace TeamChallenge.Models.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        [ForeignKey("User")]
        public required string UserId { get; set; }
        public UserEntity User { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<OrderHistoryEntity> OrderHistory { get; set; }
    }
}