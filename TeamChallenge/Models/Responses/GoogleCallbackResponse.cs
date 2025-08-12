using TeamChallenge.Models.Responses.GoogleResponses;

namespace TeamChallenge.Models.Responses
{
    public class GoogleCallbackResponse: Response
    {
        public OAuthGoogleResponse AuthGoogleResponse { get; set; }
        public TokenResponse TokenResponse { get; set; }
    }
}
