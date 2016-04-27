using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Routing.Template;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Metrics.Infrastructure
{
    public class DefaultRouteTemplateResolver : IRouteNameResolver
    {
        public async Task<string> ResolveMatchingAttributeRoute(RouteContext routeContext)
        {
            var actionSelector = routeContext.HttpContext.RequestServices.GetRequiredService<IActionSelector>();

            var actionDescriptor = await actionSelector.SelectAsync(routeContext);

            return actionDescriptor?.AttributeRouteInfo?.Template.ToLower() ?? string.Empty;
        }

        public Task<string> ResolveMatchingTemplateRoute(RouteContext routeContext)
        {
            var templateRoute = routeContext.RouteData.Routers
                .FirstOrDefault(r => r.GetType().Name == "TemplateRoute")
                as TemplateRoute;

            if (templateRoute == null) return Task.FromResult(string.Empty);

            var controller = routeContext.RouteData.Values.FirstOrDefault(v => v.Key == "controller");
            var action = routeContext.RouteData.Values.FirstOrDefault(v => v.Key == "action");

            var result = templateRoute.ToTemplateString(controller.Value as string, action.Value as string);

            return Task.FromResult(result.ToLower());
        }
    }
}