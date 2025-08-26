using Newtonsoft.Json;

namespace TeamChallenge.Models.Responses
{
    public class OAuthGoogleResponse: BaseTokenOAuthResponse
    {
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
