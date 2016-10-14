using System;
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
        private readonly StringReport _stringReport;

        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next,
            StringReport stringReport,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            if (stringReport == null)
            {
                throw new ArgumentNullException(nameof(stringReport));
            }

            _stringReport = stringReport;
        }

        public Task Invoke(HttpContext context)
        {
            if (Options.MetricsTextEnabled && Options.MetricsTextEndpoint.HasValue && Options.MetricsTextEndpoint == context.Request.Path)
            {
                var content = _stringReport.RenderMetrics(MetricsContext.DataProvider.CurrentMetricsData, MetricsContext.GetHealthStatusAsync);
                return WriteResponseAsync(context, content, "text/plain");
            }

            return Next(context);
        }
    }
}