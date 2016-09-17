using System;
using System.Net;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class ErrorMeterMiddleware : MetricsMiddlewareBase
    {
        private readonly Meter _badRequestMeter;
        private readonly Meter _errorMeter;
        private readonly RequestDelegate _next;
        private readonly Meter _unauthorizedMeter;

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

            _errorMeter = metricsContext.Context.Context(ApplicationRequestsContextName)
                .Meter("Web Error Requests", Unit.Custom("500 Errors"));

            _badRequestMeter = metricsContext.Context.Context(ApplicationRequestsContextName)
                .Meter("Web Bad Requests", Unit.Custom("400 Errors"));

            _unauthorizedMeter = metricsContext.Context.Context(ApplicationRequestsContextName)
                .Meter("Web Unauthorized Requests", Unit.Custom("401 Errors"));
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                await _next(context);

                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.InternalServerError:
                        _errorMeter.Mark();
                        break;
                    case (int)HttpStatusCode.BadRequest:
                        _badRequestMeter.Mark();
                        break;
                    case (int)HttpStatusCode.Unauthorized:
                        _unauthorizedMeter.Mark();
                        break;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}