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
            services.TryAddSingleton<MockitDelegatingHandler>();
            services.TryAddSingleton<MockitOptions>();

            services.AddHostedService<MockitRefreshService>();

            return services;
        }
    }
}
