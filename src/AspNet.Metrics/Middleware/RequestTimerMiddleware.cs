using System;
using System.Threading.Tasks;
using Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class RequestTimerMiddleware : MetricsMiddlewareBase
    {
        private const string TimerItemsKey = "__Mertics.RequestTimer__";
        private readonly RequestDelegate _next;
        private readonly Timer _requestTimer;

        public RequestTimerMiddleware(RequestDelegate next, MetricsOptions options, AspNetMetricsContext metricsContext)
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
            _requestTimer = metricsContext.Context.Context(ApplicationRequestsContextName)
                .Timer("Web Requests", Unit.Requests);
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                context.Items[TimerItemsKey] = _requestTimer.NewContext();

                await _next(context);

                var timer = context.Items[TimerItemsKey];
                using (timer as IDisposable)
                {
                }
                context.Items.Remove(TimerItemsKey);
            }
            else
            {
                await _next(context);
            }
        }
    }
}