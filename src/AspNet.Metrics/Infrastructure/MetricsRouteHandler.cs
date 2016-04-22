using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Routing;

namespace AspNet.Metrics.Infrastructure
{
    public class MetricsRouteHandler : IRouter
    {
        private readonly IRouter _requestRouter;
        private readonly IRouteNameResolver _routeNameResolver;


        public MetricsRouteHandler(IRouter requestRouter)
        {
            if (requestRouter == null)
            {
                throw new ArgumentNullException(nameof(requestRouter));
            }

            _requestRouter = requestRouter;
            _routeNameResolver = new DefaultRouteNameResolver();
        }

        public MetricsRouteHandler(IRouter requestRouter, IRouteNameResolver routeNameResolver)
        {
            if (requestRouter == null)
            {
                throw new ArgumentNullException(nameof(requestRouter));
            }
            if (routeNameResolver == null)
            {
                throw new ArgumentNullException(nameof(routeNameResolver));
            }

            _requestRouter = requestRouter;
            _routeNameResolver = routeNameResolver;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            // We return null here because we're not responsible for generating the url, the route is.
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var metricName = await _routeNameResolver.Resolve(context);

            if (!string.IsNullOrEmpty(metricName))
            {
                context.AddMetricsCurrentRouteName(metricName);
                await _requestRouter.RouteAsync(context);
            }

            //Fall back to the default which looks at the route group value on the route context set my asp.net routing
            if (_routeNameResolver.GetType() != typeof(DefaultRouteNameResolver))
            {
                metricName = await _routeNameResolver.Resolve(context);

                if (!string.IsNullOrEmpty(metricName))
                {
                    context.AddMetricsCurrentRouteName(metricName);
                }
            }

            await _requestRouter.RouteAsync(context);
        }
    }
}