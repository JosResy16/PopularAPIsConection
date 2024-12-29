using Microsoft.AspNetCore.Mvc;
using PopularAPIsConection.Services.SpotifyAPI;

namespace PopularAPIsConection.Controllers.Spotify
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SpotifyService _SpotifyService;

        public SpotifyController(IConfiguration configuration, SpotifyService spotifyService)
        {
            _configuration = configuration;
            _SpotifyService = spotifyService;
        }

        [HttpGet("artist-id")]
        public async Task<IActionResult> GetArtistById()
        {
            var artistId = "0TnOYISbd1XYRBk9myaseg";

            var artist = await _SpotifyService.GetArtistById(artistId);

            return Ok(artist);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCatogories()
        {
            var listOfCategories = await _SpotifyService.GetListCateogories();

            return Ok(listOfCategories);
        }

        //no hay permisos para acceder a la información del usaurio
        [HttpGet("user-data")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _SpotifyService.GetUserInfo();

            return Ok(userInfo);
        }
    }
}
