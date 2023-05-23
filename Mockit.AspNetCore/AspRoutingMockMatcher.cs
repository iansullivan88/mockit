using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mockit.AspNetCore.RouteConstraints;
using System;

namespace Mockit.AspNetCore
{
    public class AspRoutingMockMatcher : IMockMatcher
    {
        private const string MockRouteDataKey = "mock";

        private static readonly RouteHandler EmptyRouteHandler = new RouteHandler(ctx => Task.CompletedTask);

        private static readonly DefaultInlineConstraintResolver ConstraintResolver;

        private static readonly ServiceProvider RoutingServiceProvider;

        private RouteCollection _routeCollection = new RouteCollection();


        static AspRoutingMockMatcher()
        {
            var services = new ServiceCollection();
            services.AddLogging();

            RoutingServiceProvider = services.BuildServiceProvider();
            ConstraintResolver = new DefaultInlineConstraintResolver(Options.Create(new RouteOptions()), RoutingServiceProvider);
        }

        public AspRoutingMockMatcher()
        {
        }

        public HttpMock? GetMatchingMock(HttpRequestMessage request)
        {
            if (request.RequestUri == null)
            {
                return null;
            }

            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Request.Method = request.Method.ToString();
            mockHttpContext.Request.Host = new HostString(request.RequestUri.Host);
            mockHttpContext.Request.Path = request.RequestUri.AbsolutePath;
            mockHttpContext.RequestServices = RoutingServiceProvider;

            var routeContext = new RouteContext(mockHttpContext);

            _routeCollection.RouteAsync(routeContext).Wait();

            return routeContext.RouteData.DataTokens[MockRouteDataKey] as HttpMock;
        }

        public void Rebuild(ICollection<HttpMock> mocks)
        {
            var newRouteCollection = new RouteCollection();
            foreach (var mock in mocks.Where(m => m.Matching.Enabled))
            {
                var route = GetRouteFromMock(mock);
                newRouteCollection.Add(route);
            }

            _routeCollection = newRouteCollection;
        }

        private static Route GetRouteFromMock(HttpMock mock)
        {
            return new Route(
                target: EmptyRouteHandler,
                routeName: mock.Id.ToString(),
                routeTemplate: mock.Matching.Path,
                defaults: null,
                constraints: new Dictionary<string, object>
                {
                    ["httpMethod"] = new HttpMethodRouteConstraint(new[] { mock.Matching.Method.ToString() }),
                    ["host"] = new HostRouteConstraint(mock.Matching.Host)
                },
                dataTokens: new RouteValueDictionary
                {
                    [MockRouteDataKey] = mock,
                },
                inlineConstraintResolver: ConstraintResolver);
        }
    }
}
