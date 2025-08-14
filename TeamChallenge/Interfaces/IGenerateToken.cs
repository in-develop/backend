using TeamChallenge.Models.Entities;

namespace TeamChallenge.Interfaces
{
    public interface IGenerateToken
    {
        (string, DateTime)GenerateToken(User user, IList<string> roles);
    }
}
