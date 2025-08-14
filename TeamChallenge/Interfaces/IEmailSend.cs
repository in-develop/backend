using TeamChallenge.Models.Entities;

namespace TeamChallenge.Interfaces
{
    public interface IEmailSend
    {
        Task SendEmail(string email, string body);
    }
}
