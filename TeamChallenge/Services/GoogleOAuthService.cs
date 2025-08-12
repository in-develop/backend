using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TeamChallenge.Helpers;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Responses.GoogleResponses;

namespace TeamChallenge.Services
{
    public class GoogleOAuthService : IGoogleOAuth
    {
        private readonly IConfiguration _configuration;
        public GoogleOAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string state)
        {
            var oAuthEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

            var queryParams = new Dictionary<string, string>
            {
                {"client_id",  _configuration["Authentication:Google:ClientId"]},
                {"redirect_uri", redirectUrl },
                {"response_type", "code" },
                {"scope", scope },
                {"code_challenge", codeChellange },
                {"code_challenge_method", "S256" },
                {"state", state},
                {"access_type", "offline" },
                {"prompt", "consent"}
            };

            var url = QueryHelpers.AddQueryString(oAuthEndpoint, queryParams);
            return url;
        }

        public async Task<OAuthGoogleResponse> ExchangeCodeOnToken(string code, string codeVerifier, string redicertUrl)
        {
            var tokenEndpoint = "https://oauth2.googleapis.com/token";

            var authParams = new Dictionary<string, string>
            {
                {"client_id", _configuration["Authentication:Google:ClientId"] },
                {"client_secret", _configuration["Authentication:Google:ClientSecret"] },
                {"code", code },
                {"code_verifier", codeVerifier },
                {"grant_type", "authorization_code"},
                {"redirect_uri", redicertUrl }
            };
            var tokenResult = await HttpClientHelper<OAuthGoogleResponse>.SendPostRequest(tokenEndpoint, authParams);
            return tokenResult;
        }        

        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            var refreshParams = new Dictionary<string, string>
            {
                {"client_id", _configuration["Authentication:Google:ClientId"] },
                {"client_secret", _configuration["Authentication:Google:ClientSecret"] },
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken }
            };

            var tokenResult = await HttpClientHelper<TokenResponse>.SendPostRequest("https://oauth2.googleapis.com/token", refreshParams);

            return tokenResult;
        }
    }
}
