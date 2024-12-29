
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PopularAPIsConection.Services.SpotifyAPI
{
    public class SpotifyAuthHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private string? _cachedToken;
        private DateTime? _tokenExpiration;

        public SpotifyAuthHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_cachedToken == null || DateTime.UtcNow >= _tokenExpiration)
            {
                (_cachedToken, _tokenExpiration) = await RequestNewTokenAsync();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _cachedToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<(string token, DateTime expiration)> RequestNewTokenAsync()
        {
            var clientID = _configuration["SpotifyCredentials:ClientID"];
            var clientSecret = _configuration["SpotifyCredentials:ClientSecret"];

            _httpClient.BaseAddress = new Uri("https://accounts.spotify.com/");

            var requestToken = new HttpRequestMessage(HttpMethod.Post, "api/token");

            requestToken.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientID}:{clientSecret}")));

            requestToken.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" }
            });

            var response = await _httpClient.SendAsync(requestToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AccessToken>(content);

            return (tokenResponse.Token, DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn));
        }
    }
}
