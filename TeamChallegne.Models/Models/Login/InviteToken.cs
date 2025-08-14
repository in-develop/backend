using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Login
{
    public class InviteToken
    {
        public int id { get; set; }
        public string Email { get; set; }
        public int Token { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserEntity User { get; set; }
    }
}
