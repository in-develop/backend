using TeamChallenge.Models.Entities;

namespace TeamChallenge.Services
{
    public interface IEmailSend
    {
        Task SendEmail(string email, string body);
    }
}
