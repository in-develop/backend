using Newtonsoft.Json;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Helpers
{
    public static class HttpClientHelper<T>
    {
        public static async Task<T> SendPostRequest(string tokenEndpoint, Dictionary<string, string> authParams)
        {
            using (var client = new HttpClient())
            {
                var requestData = new FormUrlEncodedContent(authParams);
                var response = await client.PostAsync(tokenEndpoint, requestData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorBody);
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenData = JsonConvert.DeserializeObject<T>(jsonResponse);
                if (tokenData == null)
                {
                    Console.WriteLine("Error: tokenData is null");
                }

                return tokenData;
            }
        }
    }
}
