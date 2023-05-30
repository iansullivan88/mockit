using Mockit.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMockit();
builder.Services.AddControllers();

builder.Services
    .AddHttpClient(string.Empty)
    .AddHttpMessageHandler<MockitDelegatingHandler>();

var app = builder.Build();

app.UseMockitUi();

app.MapControllers();

var manager = app.Services.GetRequiredService<IMockitManager>();

await manager.SaveMockAsync(new HttpMock(
    id: Guid.NewGuid(),
    matching: new HttpMockMatching(
        enabled: true,
        method: "GET",
        host: "wttr.in",
        path: "/phoenix"),
    response: new HttpMockResponse(
        statusCode: 200,
        headers: new Dictionary<string, string>(),
        content: Encoding.UTF8.GetBytes("{ \"current_condition\": [ { \"precipMM\": \"1.0\", \"windspeedMiles\": \"4\" } ] }")),
    lastModified: DateTime.UtcNow));

await manager.SaveMockAsync(new HttpMock(
    id: Guid.NewGuid(),
    matching: new HttpMockMatching(
        enabled: true,
        method: "GET",
        host: "wttr.in",
        path: "/{city}"),
    response: new HttpMockResponse(
        statusCode: 200,
        headers: new Dictionary<string, string>(),
        content: Encoding.UTF8.GetBytes("{ \"current_condition\": [ { \"precipMM\": \"1.0\", \"windspeedMiles\": \"4\" } ] }")),
    lastModified: DateTime.UtcNow));

app.Run();