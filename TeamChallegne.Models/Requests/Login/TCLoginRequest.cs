namespace TeamChallenge.Models.Requests.Login
{
    public class TCLoginRequest
    {        
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
