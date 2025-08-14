namespace TeamChallenge.Models.Login
{
    public class LoginModel
    {
        public string? TokenString { get; set; }
        public DateTime Expires { get; set; }
    }
}
