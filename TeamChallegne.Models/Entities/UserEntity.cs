using Microsoft.AspNetCore.Identity;

namespace TeamChallenge.Models.Entities
{
    public class UserEntity : IdentityUser
    {
        public CartEntity Cart { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
