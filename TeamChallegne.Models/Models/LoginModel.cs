
namespace TeamChallenge.Models.Responses
{
    public class LoginModel
    {
        public string? TokenString { get; set; }
        public DateTime Expires { get; set; }
    }
}
