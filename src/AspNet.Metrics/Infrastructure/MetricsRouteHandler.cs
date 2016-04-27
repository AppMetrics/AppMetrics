using System;
using System.Threading.Tasks;
using AspNet.Metrics.Internal;
using AspNet.Metrics.Logging;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Metrics.Infrastructure
{
    public class MetricsRouteHandler : IRouter
    {
        private readonly IRouter _next;
        private readonly IRouteNameResolver _routeNameResolver;
        private IActionSelector _actionSelector;
        private ILogger _logger;


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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var services = context.HttpContext.RequestServices;

            // Verify if AddMetrics was done before calling UseMetricsWithMvc
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(services);
            EnsureServices(context.HttpContext);

            var template = await _routeNameResolver.ResolveMatchingTemplateRoute(context);

            if (!string.IsNullOrWhiteSpace(template))
            {
                _logger.TemplateRouteFound(template);
                await ContinueNext(context, template);
                return;
            }

            template = await _routeNameResolver.ResolveMatchingAttributeRoute(context);

            if (!string.IsNullOrWhiteSpace(template))
            {
                _logger.AttributeTemplateFound(template);
                await ContinueNext(context, template);
                return;
            }

            await _next.RouteAsync(context);
        }

        private async Task ContinueNext(RouteContext context, string template)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await _next.RouteAsync(context);

            if (!context.IsHandled)
            {
                return;
            }

            if (!string.IsNullOrEmpty(template))
            {
                context.AddMetricsCurrentRouteName(template);
            }
        }

        private void EnsureServices(HttpContext context)
        {
            if (_actionSelector == null)
            {
                _actionSelector = context.RequestServices.GetRequiredService<IActionSelector>();
            }

            if (_logger == null)
            {
                var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                _logger = factory.CreateLogger<MetricsRouteHandler>();
            }
        }
    }
}