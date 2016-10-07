using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Reporters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
        }

        public Task Invoke(HttpContext context)
        {
            if (Options.MetricsTextEnabled && Options.MetricsTextEndpoint.HasValue && Options.MetricsTextEndpoint == context.Request.Path)
            {
                var content = StringReport.RenderMetrics(MetricsContext.DataProvider.CurrentMetricsData, MetricsContext.HealthStatus);
                return WriteResponse(context, content, "text/plain");
            }

            return Next(context);
        }
    }
}