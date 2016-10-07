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
        private readonly IMetricsJsonBuilder _jsonBuilder;

        public MetricsEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricsJsonBuilder jsonBuilder)
            : base(next, options, loggerFactory, metricsContext)
        {
            _jsonBuilder = jsonBuilder;
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (Options.MetricsEnabled && Options.MetricsEndpoint.HasValue && Options.MetricsEndpoint == context.Request.Path)
            {
                var json = _jsonBuilder.BuildJson(MetricsContext.DataProvider.CurrentMetricsData);

                //TODO: AH - can't hard code the schme version here
                return WriteResponse(context, json, MetricsJsonBuilderV1.MetricsMimeType);
            }

            return _next(context);
        }
    }
}