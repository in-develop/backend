using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class CartEntity : BaseEntity
    {
        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public UserEntity? User { get; set; }
        public List<CartItemEntity> CartItems { get; set; }
    }
}