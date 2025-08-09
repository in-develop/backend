namespace TeamChallenge.Models.Responses
{
    public class LoginResponse: Response
    {
        public string? TokenString { get; set; }
        public DateTime Expires { get; set; }
    }
}
