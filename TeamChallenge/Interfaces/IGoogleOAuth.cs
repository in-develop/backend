using TeamChallenge.Models.Responses.GoogleResponses;

namespace TeamChallenge.Interfaces
{
    public interface IGoogleOAuth
    {
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string state);

        public Task<OAuthGoogleResponse> ExchangeCodeOnToken(string code, string codeVerifier, string redicertUrl);

        public Task<TokenResponse> RefreshToken(string refreshToken);
    }
}
