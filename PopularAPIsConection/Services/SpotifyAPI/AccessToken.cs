using System.Text.Json.Serialization;

namespace PopularAPIsConection.Services.SpotifyAPI
{

    public class AccessToken
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

}
