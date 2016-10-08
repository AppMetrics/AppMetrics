using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class PingEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        public PingEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
        }

        public Task Invoke(HttpContext context)
        {
            if (Options.PingEnabled && Options.PingEndpoint.HasValue && Options.PingEndpoint == context.Request.Path)
            {
                return WriteResponseAsync(context, "pong", "text/plain");
            }

            return Next(context);
        }
    }
}