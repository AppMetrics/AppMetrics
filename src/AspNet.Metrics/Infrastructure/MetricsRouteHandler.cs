using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Routing;

namespace AspNet.Metrics.Infrastructure
{
    public class MetricsRouteHandler : IRouter
    {
        private readonly IRouter _next;
        private readonly IRouteNameResolver _routeNameResolver;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MetricsRouteHandler" /> class.
        /// </summary>
        /// <param name="next">The request router.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public MetricsRouteHandler(IRouter next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
            _routeNameResolver = new DefaultRouteTemplateResolver();
        }

        public MetricsRouteHandler(IRouter next,
            IRouteNameResolver routeNameResolver)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (routeNameResolver == null)
            {
                throw new ArgumentNullException(nameof(routeNameResolver));
            }

            _next = next;
            _routeNameResolver = routeNameResolver;
        }

        /// <inheritdoc />
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
            => _next.GetVirtualPath(context);


        /// <inheritdoc />
        public async Task RouteAsync(RouteContext context)
        {
            var metricName = await _routeNameResolver.ResolveMatchingTemplateRoute(context);

            if (string.IsNullOrWhiteSpace(metricName))
            {
                metricName = await _routeNameResolver.ResolveMatchingAttributeRoute(context);
            }

            await _next.RouteAsync(context);

            if (!context.IsHandled)
            {
                return;
            }

            if (!string.IsNullOrEmpty(metricName))
            {
                context.AddMetricsCurrentRouteName(metricName);
            }
        }
    }
}