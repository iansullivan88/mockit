using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Mockit.AspNetCore.RouteConstraints
{
    public class HostRouteConstraint : IRouteConstraint
    {
        private readonly string _host;

        public HostRouteConstraint(string host)
        {
            _host = host;
        }

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _host == httpContext?.Request.Host.Host;
        }
    }
}
