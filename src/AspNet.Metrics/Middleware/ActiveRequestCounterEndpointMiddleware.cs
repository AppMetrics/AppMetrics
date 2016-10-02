using System;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class ActiveRequestCounterEndpointMiddleware : MetricsMiddlewareBase
    {
        private readonly Counter _activeRequests;
        private readonly RequestDelegate _next;

        public ActiveRequestCounterEndpointMiddleware(RequestDelegate next, AspNetMetricsOptions options, AspNetMetricsContext metricsContext)
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
            _activeRequests = metricsContext.Context.GetWebApplicationContext()
                .Counter("Active Requests", Unit.Custom("ActiveRequests"));
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                _activeRequests.Increment();

                await _next(context);

                _activeRequests.Decrement();
            }
            else
            {
                await _next(context);
            }
        }
    }
}