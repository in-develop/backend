using Newtonsoft.Json;

namespace TeamChallenge.Models.Responses.GoogleResponses
{
    public class TokenResponse: BaseTokenOAuthResponse
    {

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
