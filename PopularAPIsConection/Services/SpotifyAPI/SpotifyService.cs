using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PopularAPIsConection.Services.SpotifyAPI
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SpotifyClient");
            _httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
        }

        public async Task<Artist> GetArtistById(string artistID)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/artist/{artistID}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var artist = JsonSerializer.Deserialize<Artist>(responseContent);
            return artist;
        }

        public async Task<List<string>> GetListCateogories()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"browse/categories?limit=5");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);

            var categories = json["categories"]["items"]
                  .Select(item => item["name"].ToString())
                  .ToList();

            return categories;
        }

        //no existen permisos para acceder a información del usaurio
        public async Task<UserProfileData> GetUserInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "me");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var userData = JsonSerializer.Deserialize<UserProfileData>(responseContent);

            return userData;
        }
    }
}
