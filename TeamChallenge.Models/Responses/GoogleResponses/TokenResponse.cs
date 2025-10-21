using Newtonsoft.Json;

namespace TeamChallenge.Models.Responses
{
    public class TokenResponse: BaseTokenOAuthResponse
    {

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
