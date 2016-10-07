using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class PostAndPutRequestSizeHistogramMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly IHistogram _histogram;

        public PostAndPutRequestSizeHistogramMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _histogram = MetricsContext.GetWebApplicationContext()
                .Histogram("Web Request Post & Put Size", Unit.Bytes);
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                var httpMethod = context.Request.Method.ToUpperInvariant();

                if (httpMethod == "POST" || httpMethod == "PUT")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        _histogram.Update(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }
            }

            await Next(context);
        }
    }
}