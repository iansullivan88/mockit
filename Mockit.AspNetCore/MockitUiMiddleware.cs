﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Mockit.AspNetCore
{
    public class MockitUiMiddleware
    {
        private readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly RequestDelegate _next;
        private readonly MockitOptions _options;
        private readonly IMockitManager _mockitManager;
        private readonly StaticFileMiddleware _staticMiddleware;

        public MockitUiMiddleware(
            RequestDelegate next,
            IMockitManager mockitManager,
            MockitOptions options,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _mockitManager = mockitManager;
            _options = options;

            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = options.UiPrefix,
                FileProvider = new EmbeddedFileProvider(typeof(MockitUiMiddleware).GetTypeInfo().Assembly, "Mockit.AspNetCore.Ui"),
            };

            _staticMiddleware = new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (!(path?.StartsWith(_options.UiPrefix) ?? false))
            {
                await _next(context);
                return;
            }

            if (context.Request.Method == "GET" && path == $"{_options.UiPrefix}/mocks")
            {
                var mocks = _mockitManager.GetMocks();
                var entities = mocks.Select(HttpMock.ToEntity);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.Response.WriteAsJsonAsync(entities);
            }
            else if (context.Request.Method == "PUT" && path == $"{_options.UiPrefix}/mocks")
            {
                var mockEntity = await JsonSerializer.DeserializeAsync<HttpMockEntity>(context.Request.Body, JsonOptions);
                mockEntity = mockEntity! with { LastModified = DateTime.UtcNow };
                var mock = HttpMock.FromEntity(mockEntity!);
                await _mockitManager.SaveMockAsync(mock);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                await _staticMiddleware.Invoke(context);
            }
        }
    }
}
