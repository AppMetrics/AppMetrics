using System;
using System.Linq;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class PostAndPutRequestSizeHistogramMiddleware : MetricsMiddlewareBase
    {
        private readonly Histogram _histogram;
        private readonly RequestDelegate _next;

        public PostAndPutRequestSizeHistogramMiddleware(RequestDelegate next, MetricsOptions options, AspNetMetricsContext metricsContext)
            : base(options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _histogram = metricsContext.Context.Context(ApplicationRequestsContextName)
                .Histogram("Web Request Post & Put Size", Unit.Bytes, SamplingType.Default);
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

            await _next(context);
        }
    }
}