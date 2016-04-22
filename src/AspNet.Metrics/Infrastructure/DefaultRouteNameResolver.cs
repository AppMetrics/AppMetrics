using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Routing;

namespace AspNet.Metrics.Infrastructure
{
    public class DefaultRouteNameResolver : IRouteNameResolver
    {
        public Task<string> Resolve(RouteContext routeContext)
        {
            var routeName = string.Empty;

            if (routeContext.RouteData.Values.ContainsKey("!__route_group"))
            {
                routeName = routeContext.RouteData.Values["!__route_group"] as string;
            }

            return Task.FromResult(routeName);
        }
    }
}