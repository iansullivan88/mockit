using System.Net.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/global-times", async (HttpClient httpClient) =>
{
    var nyTime = await GetTime(httpClient, "America/New_York");
    var londonTime = await GetTime(httpClient, "Europe/London");
    var tokyoTime = await GetTime(httpClient, "Asia/Tokyo");

    return new
    {
        newYork = new { nyTime.dateTime, nyTime.timezone },
        london = new { londonTime.dateTime, londonTime.timezone },
        tokyo = new { tokyoTime.dateTime, tokyoTime.timezone }
    };
});

app.Run();

static async Task<(string timezone, string dateTime)> GetTime(HttpClient httpClient, string timezone)
{
    var response = await httpClient.GetAsync($"https://worldtimeapi.org/api/timezone/{timezone}");
    response.EnsureSuccessStatusCode();

    using var responseStream = await response.Content.ReadAsStreamAsync();
    var jsonDocument = await JsonDocument.ParseAsync(responseStream);

    var rootElement = jsonDocument.RootElement;

    return (
        timezone: rootElement.GetProperty("timezone").GetString()!,
        dateTime: rootElement.GetProperty("datetime").GetString()!
    );
}