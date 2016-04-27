using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspNet.Metrics.Internal;
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
        private IActionContextAccessor _actionContextAccessor;
        private IActionSelector _actionSelector;
        private DiagnosticSource _diagnosticSource;
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

        private void EnsureServices(HttpContext context)
        {
            if (_actionContextAccessor == null)
            {
                _actionContextAccessor = context.RequestServices.GetRequiredService<IActionContextAccessor>();
            }

            if (_actionSelector == null)
            {
                _actionSelector = context.RequestServices.GetRequiredService<IActionSelector>();
            }

            if (_logger == null)
            {
                var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                _logger = factory.CreateLogger<MetricsRouteHandler>();
            }

            if (_diagnosticSource == null)
            {
                _diagnosticSource = context.RequestServices.GetRequiredService<DiagnosticSource>();
            }
        }
    }
}