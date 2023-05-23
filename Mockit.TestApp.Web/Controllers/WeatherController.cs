using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Mockit.TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            var response = await _httpClient.GetAsync($"https://wttr.in/{city}?format=j1");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var jsonDocument = await JsonDocument.ParseAsync(responseStream);

            var rootElement = jsonDocument.RootElement;
            var currentConditions = rootElement.GetProperty("current_condition")[0];

            var precipitationInMm = double.Parse(currentConditions.GetProperty("precipMM").GetString()!);
            var windSpeedMiles = double.Parse(currentConditions.GetProperty("windspeedMiles").GetString()!);

            return Json(new
            {
                shouldIUseAnUmbrella = precipitationInMm >= 0.1 && windSpeedMiles < 15
            });
        }
    }
}
