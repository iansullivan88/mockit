using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mockit.AspNetCore
{
    public static class MockitExtensions
    {
        public static IServiceCollection AddMockit(
            this IServiceCollection services,
            MockitOptions? options = null)
        {
            options ??= new MockitOptions();

            services.TryAddSingleton<IMockitManager, MockitManager>();
            services.TryAddSingleton<IMockitStore, InMemoryMockitStore>();
            services.TryAddSingleton<IMockMatcher, AspRoutingMockMatcher>();
            services.TryAddSingleton(options);
            services.TryAddTransient<MockitDelegatingHandler>();

            services.AddHostedService<MockitRefreshService>();

            return services;
        }

        public static IApplicationBuilder UseMockitUi(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MockitUiMiddleware>();
        }
    }
}
