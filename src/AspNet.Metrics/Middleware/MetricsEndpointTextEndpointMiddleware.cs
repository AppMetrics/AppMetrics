using System;
using System.Threading.Tasks;
using App.Metrics.Reporters;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointTextEndpointMiddleware : MetricsEndpointMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;
        private readonly AspNetMetricsOptions _options;

        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next, AspNetMetricsOptions options, AspNetMetricsContext metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _next = next;
            _options = options;
            _metricsContext = metricsContext;
        }

        public Task Invoke(HttpContext context)
        {
            if (_options.MetricsTextEnabled && _options.MetricsTextEndpoint.HasValue && _options.MetricsTextEndpoint == context.Request.Path)
            {
                var content = StringReport.RenderMetrics(_metricsContext.Context.DataProvider.CurrentMetricsData, _metricsContext.HealthStatus);
                return WriteResponse(context, content, "text/plain");
            }

            return _next(context);
        }
    }
}