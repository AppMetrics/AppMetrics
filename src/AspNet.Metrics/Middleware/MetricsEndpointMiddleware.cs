using System;
using System.Threading.Tasks;
using Metrics.Json;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointMiddleware : MetricsEndpointMiddlewareBase
    {
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;
        private readonly MetricsOptions _options;

        public MetricsEndpointMiddleware(RequestDelegate next, MetricsOptions options, 
            AspNetMetricsContext metricsContext)
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
            if (_options.MetricsEnabled && _options.MetricsEndpoint.HasValue && _options.MetricsEndpoint == context.Request.Path)
            {
                var json = JsonBuilderV2.BuildJson(_metricsContext.Context.DataProvider.CurrentMetricsData);

                return WriteResponse(context, json, JsonBuilderV2.MetricsMimeType);
            }

            return _next(context);
        }
    }
}