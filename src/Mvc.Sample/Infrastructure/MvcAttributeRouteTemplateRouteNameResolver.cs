using System.Threading.Tasks;
using AspNet.Metrics.Infrastructure;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Mvc.Sample.Infrastructure
{

    public class MvcAttributeRouteTemplateRouteNameResolver : IRouteNameResolver
    {
        public async Task<string> Resolve(RouteContext routeContext)
        {
            var actionSelector = routeContext.HttpContext.RequestServices.GetRequiredService<IActionSelector>();

            var actionDescriptor = await actionSelector.SelectAsync(routeContext);

            if (actionDescriptor?.AttributeRouteInfo != null)
            {
                // Route template will be null if attribute routing is not used
                return actionDescriptor.AttributeRouteInfo.Template;
            }

            return string.Empty;
        }
    }
}