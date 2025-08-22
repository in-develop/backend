using TeamChallenge.Models.Entities;

namespace TeamChallenge.Services
{
    public interface IGenerateToken
    {
        (string, DateTime)GenerateToken(UserEntity user, IList<string> roles);
    }
}
