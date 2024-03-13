

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BetterSpotifySearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SongController : ControllerBase
    {
        private readonly IAccessService _accessService;

        public SongController(IAccessService accessService){
            _accessService = accessService;
        }

        [HttpGet]
        public async Task<IActionResult> SharedServiceTest()
        {
            return(Ok(_AccessService.GetTestToken()));
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Songs(string? name)
        {
            using HttpClient httpClient = new HttpClient();
            string? _accessToken = _AccessService.GetAccessToken();

            if(name == null)
            {
                return BadRequest("No search value");
            }
            StringBuilder requestBuilder = new StringBuilder("https://api.spotify.com/v1/search?");
            requestBuilder.Append("q=" + Uri.EscapeDataString(name));
            requestBuilder.Append("&type=track");
            requestBuilder.Append("&limit=10");
            string requestString = requestBuilder.ToString();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await httpClient.SendAsync(requestMessage);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            return BadRequest(response.ReasonPhrase);
        }
    }
}
