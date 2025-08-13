using Microsoft.AspNetCore.Identity;

namespace TeamChallenge.Models.Models
{
    public class User : IdentityUser
    {
        public bool IsEmailConfirmed { get; set; }
    }
}
