using System;
using System.Threading.Tasks;
using App.Metrics.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Infrastructure
{
    public class MetricsResourceFilter : IAsyncResourceFilter
    {
        private readonly IRouteNameResolver _routeNameResolver;
        private ILogger _logger;

        public MetricsResourceFilter(IRouteNameResolver routeNameResolver)
        {
            if (routeNameResolver == null)
            {
                throw new ArgumentNullException(nameof(routeNameResolver));
            }

            _routeNameResolver = routeNameResolver;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Verify if AddMetrics was done before calling UseMetricsWithMvc
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(context.HttpContext.RequestServices);
            EnsureServices(context.HttpContext);

            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRoute(context.RouteData);

            if (!string.IsNullOrEmpty(templateRoute))
            {
                context.HttpContext.AddMetricsCurrentRouteName(templateRoute);
            }

            await next.Invoke();
        }

        private void EnsureServices(HttpContext context)
        {
            if (_logger == null)
            {
                var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                _logger = factory.CreateLogger<MetricsResourceFilter>();
            }
        }
    }
}