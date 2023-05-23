using Mockit.AspNetCore;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMockit();

builder.Services
    .AddHttpClient(string.Empty)
    .AddHttpMessageHandler<MockitDelegatingHandler>();

var app = builder.Build();

var manager = app.Services.GetRequiredService<IMockitManager>();
manager.SaveMockAsync(new HttpMock(
    id: Guid.NewGuid(),
    matching: new HttpMockMatching(
        enabled: false,
        method: "GET",
        host: "worldtimeapi.org",
        path: "/api/timezone/America/New_York"),
    response: new HttpMockResponse(
        statusCode: 200,
        headers: new Dictionary<string, string>(),
        content: Encoding.UTF8.GetBytes("{\"datetime\":\"2023-03-12T10:00:00\"}")),
    lastModified: DateTime.UtcNow));

app.MapGet("/global-times", async (HttpClient httpClient) =>
{
    var response = await httpClient.GetAsync($"https://worldtimeapi.org/api/timezone/America/New_York");
    response.EnsureSuccessStatusCode();

    using var responseStream = await response.Content.ReadAsStreamAsync();
    var jsonDocument = await JsonDocument.ParseAsync(responseStream);

    var rootElement = jsonDocument.RootElement;

    return new
    {
        dateTime = rootElement.GetProperty("datetime").GetString()!
    };
});

app.Run();