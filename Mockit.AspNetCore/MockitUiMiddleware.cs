using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mockit.AspNetCore
{
    public class MockitUiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MockitOptions _options;
        private readonly StaticFileMiddleware _staticMiddleware;

        public MockitUiMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            MockitOptions options)
        {
            _next = next;
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

            await _staticMiddleware.Invoke(context);
        }
    }
}
