using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace AspNet.Metrics.Infrastructure
{
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingAttributeRoute(RouteContext routeContext);

        Task<string> ResolveMatchingTemplateRoute(RouteContext routeContext);
    }
}