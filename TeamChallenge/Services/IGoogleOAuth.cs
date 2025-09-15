using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public interface IGoogleOAuth
    {
        public IResponse GenerateOAuthRequestUrl();
        public Task<IResponse> GetGoogleAuthCallback(string code, string state);

    }
}
