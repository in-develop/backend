using TeamChallenge.Models.Models;

namespace TeamChallenge.Interfaces
{
    public interface IEmailSend
    {
        Task SendEmail(string email, string body);
    }
}
