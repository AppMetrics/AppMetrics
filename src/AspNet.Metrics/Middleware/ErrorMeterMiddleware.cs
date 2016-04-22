using System;
using System.Net;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace AspNet.Metrics.Middleware
{
    public class ErrorMeterMiddleware : MetricsMiddlewareBase
    {
        private readonly Meter _errorMeter;
        private readonly RequestDelegate _next;

        public ErrorMeterMiddleware(RequestDelegate next, MetricsOptions options, AspNetMetricsContext metricsContext)
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
            _errorMeter = metricsContext.Context.Meter("Web Request Errors", Unit.Errors);
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                await _next(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    _errorMeter.Mark();
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}