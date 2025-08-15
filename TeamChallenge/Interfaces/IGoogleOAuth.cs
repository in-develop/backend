using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.GoogleResponses;

namespace TeamChallenge.Interfaces
{
    public interface IGoogleOAuth
    {
        public IDataResponse<string> GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string state);

        public Task<IDataResponse<GoogleAuthCallback>> GetGoogleAuthCallback(string code, string codeVerifier, string redicertUrl);

    }
}
