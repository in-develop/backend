using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeamChallenge.StaticData;

namespace TeamChallenge.Models.Entities
{
    public class OrderHistoryEntity : BaseEntity
    {
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus? OldStatus { get; set; }
        [Required]
        public OrderStatus NewStatus { get; set; }
        [ForeignKey("Order")]
        public required int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}