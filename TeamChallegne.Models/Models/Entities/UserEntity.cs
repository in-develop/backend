using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class UserEntity : IdentityUser
    {
        public bool IsEmailConfirmed { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public CartEntity Cart { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
