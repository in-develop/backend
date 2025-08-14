using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class CartEntity : BaseEntity
    {
        [ForeignKey("User")]
        public required string UserId { get; set; }
        public UserEntity User { get; set; }
        public ICollection<CartItemEntity> CartItems { get; set; }
    }
}