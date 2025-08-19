using Newtonsoft.Json;

namespace TeamChallenge.Models.Responses.GoogleResponses
{
    public class OAuthGoogleResponse: BaseTokenOAuthResponse
    {
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
