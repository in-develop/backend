using TeamChallenge.Models.Entities;

namespace TeamChallenge.Services
{
    public interface IGenerateToken
    {
        string GenerateToken(UserEntity? user, IList<string> roles, bool rememberMe);
    }
}
