using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PopularAPIsConection.Services.SpotifyAPI
{
    public class SpotifyAuth
    {
        private readonly HttpClient _httpClient;

        public SpotifyAuth(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            _httpClient.BaseAddress = new Uri("https://accounts.spotify.com/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AccessToken> GetAccessToken(string userSecret, string userId)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("user_id", userId),
                new KeyValuePair<string, string>("user_secret", userSecret)
            });

            var response = await _httpClient.PostAsync("api/token", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var accesstoken = JsonSerializer.Deserialize<AccessToken>(responseContent);

            return accesstoken;
        }
    }
}
