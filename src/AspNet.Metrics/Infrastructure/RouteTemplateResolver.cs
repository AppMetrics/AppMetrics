using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Metrics.Infrastructure
{
    public class DefaultRouteTemplateResolver : IRouteNameResolver
    {
        public Task<string> ResolveMatchingAttributeRoute(RouteContext routeContext)
        {
            var actionSelector = routeContext.HttpContext.RequestServices.GetRequiredService<IActionSelector>();

            var actionDescriptor = actionSelector.Select(routeContext);

            return Task.FromResult(actionDescriptor?.AttributeRouteInfo?.Template.ToLower() ?? string.Empty);
        }

        public Task<string> ResolveMatchingTemplateRoute(RouteContext routeContext)
        {
            var templateRoute = routeContext.RouteData.Routers
                .FirstOrDefault(r => r.GetType().Name == "Route")
                as Route;

            if (templateRoute == null) return Task.FromResult(string.Empty);

            var controller = routeContext.RouteData.Values.FirstOrDefault(v => v.Key == "controller");
            var action = routeContext.RouteData.Values.FirstOrDefault(v => v.Key == "action");

            var result = templateRoute.ToTemplateString(controller.Value as string, action.Value as string);

            return Task.FromResult(result.ToLower());
        }
    }
}