using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace AspNet.Metrics.Infrastructure
{
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingTemplateRoute(RouteData routeData);
    }
}