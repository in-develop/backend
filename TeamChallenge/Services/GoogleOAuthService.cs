using Microsoft.AspNetCore.WebUtilities;
using TeamChallenge.Helpers;
using TeamChallenge.Interfaces;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.GoogleResponses;

namespace TeamChallenge.Services
{
    public class GoogleOAuthService : IGoogleOAuth
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;
        public GoogleOAuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void GenerateCodeVerifierState(out string codeVerifier, out string state, out string codeChallenge)
        {
            codeVerifier = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            state = Guid.NewGuid().ToString("N");
            codeChallenge = PkceHelper.GenerateCodeChallenge(codeVerifier);
        }

        public IDataResponse<string> GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChellange, string state)
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
            return new DataResponse<string>(url);
        }

        public async Task<IDataResponse<GoogleAuthCallback>> GetGoogleAuthCallback(string code)
        {
            var tokenEndpoint = "https://oauth2.googleapis.com/token";

            var redirectUri = _configuration["Authentication:Google:RedirectUri"];
            var codeVerifier = HttpContext.Session.GetString("code_verifier");

            var authParams = new Dictionary<string, string>
            {
                {"client_id", _configuration["Authentication:Google:ClientId"] },
                {"client_secret", _configuration["Authentication:Google:ClientSecret"] },
                {"code", code },
                {"code_verifier", codeVerifier },
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri }
            };
            var tokenResult = await HttpClientHelper<OAuthGoogleResponse>.SendPostRequest(tokenEndpoint, authParams);
            var refreshToken = await RefreshToken(tokenResult.RefreshToken);

            return new GoogleAuthCallbackResponse(new GoogleAuthCallback
            {
                AuthGoogleResponse = tokenResult,
                TokenResponse = refreshToken
            });
        }        

        private async Task<TokenResponse> RefreshToken(string refreshToken)
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
