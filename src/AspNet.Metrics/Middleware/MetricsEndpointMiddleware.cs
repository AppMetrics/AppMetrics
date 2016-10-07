using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly RequestDelegate _next;

        public MetricsEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (Options.MetricsEnabled && Options.MetricsEndpoint.HasValue && Options.MetricsEndpoint == context.Request.Path)
            {
                var json = JsonBuilderV1.BuildJson(MetricsContext.DataProvider.CurrentMetricsData);

                return WriteResponse(context, json, JsonBuilderV1.MetricsMimeType);
            }

            return _next(context);
        }
    }
}