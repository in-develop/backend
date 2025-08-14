using TeamChallenge.Models.Entities;

namespace TeamChallenge.Interfaces
{
    public interface IGenerateToken
    {
        (string, DateTime)GenerateToken(UserEntity user, IList<string> roles);
    }
}
