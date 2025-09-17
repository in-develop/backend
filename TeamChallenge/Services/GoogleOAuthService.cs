using Microsoft.AspNetCore.WebUtilities;
using Mono.TextTemplating;
using TeamChallenge.Helpers;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Services
{
    public class GoogleOAuthService : IGoogleOAuth
    {
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        public GoogleOAuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        private void GenerateCodeVerifierState(out string codeVerifier, out string state, out string codeChallenge)
        {
            codeVerifier = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            state = Guid.NewGuid().ToString("N");
            codeChallenge = PkceHelper.GenerateCodeChallenge(codeVerifier);
        }

        public IResponse GenerateOAuthRequestUrl()
        {
            var scope = "openid";
            var redirectUrl = _configuration["Authentication:Google:RedirectUri"];

            GenerateCodeVerifierState(out string codeVerifier, out string state, out string codeChallenge);
            _httpContext.Session.SetString("code_verifier", codeVerifier);
            _httpContext.Session.SetString("oauth_state", state);

            var oAuthEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

            var queryParams = new Dictionary<string, string>
            {
                {"client_id",  _configuration["Authentication:Google:ClientId"]!},
                {"redirect_uri", redirectUrl },
                {"response_type", "code" },
                {"scope", scope },
                {"code_challenge", codeChallenge },
                {"code_challenge_method", "S256" },
                {"state", state},
                {"access_type", "offline" },
                {"prompt", "consent"}
            };

            var url = QueryHelpers.AddQueryString(oAuthEndpoint, queryParams!);
            return new DataResponse<string>(url);
        }

        public async Task<IResponse> GetGoogleAuthCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new ErrorResponse("Authorization code missing.");
            }

            var savedState = _httpContext.Session.GetString("oauth_state");
            if (state != savedState)
            {
                return new ErrorResponse("Invalid state value.");
            }

            var tokenEndpoint = "https://oauth2.googleapis.com/token";

            var redirectUri = _configuration["Authentication:Google:RedirectUri"];
            var codeVerifier = _httpContext.Session.GetString("code_verifier");

            var authParams = new Dictionary<string, string>
            {
                {"client_id", _configuration["Authentication:Google:ClientId"]! },
                {"client_secret", _configuration["Authentication:Google:ClientSecret"]! },
                {"code", code },
                {"code_verifier", codeVerifier! },
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri! }
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
                {"client_id", _configuration["Authentication:Google:ClientId"]! },
                {"client_secret", _configuration["Authentication:Google:ClientSecret"]! },
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken }
            };

            var tokenResult = await HttpClientHelper<TokenResponse>.SendPostRequest("https://oauth2.googleapis.com/token", refreshParams);

            return tokenResult;
        }
    }
}
