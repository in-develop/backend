using Microsoft.AspNetCore.Identity;

namespace TeamChallenge.Models.Entities
{
    public class User : IdentityUser
    {
        public bool IsEmailConfirmed { get; set; }
    }
}
