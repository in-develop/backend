namespace TeamChallenge.Models.SendEmailModels
{
    public class SenderModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public int Timeout { get; set; }
    }
}
