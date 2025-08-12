namespace TeamChallenge.Models.Models
{
    public class InviteToken
    {
        public int id { get; set; }
        public string Email { get; set; }
        public int Token { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiresAt { get; set; }
        public User User { get; set; }
    }
}
