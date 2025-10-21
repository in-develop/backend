using Microsoft.AspNetCore.Identity;

namespace TeamChallenge.Models.Entities
{
    public class UserEntity : IdentityUser
    {
        public CartEntity Cart { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
        public ICollection<ReviewEntity> Reviews { get; set; }
        public DateTime SentEmailTime { get; set; }
    }
}
