using TeamChallenge.Models.Models;

namespace TeamChallenge.Interfaces
{
    public interface IGenerateToken
    {
        (string, DateTime)GenerateToken(User user, IList<string> roles);
    }
}
