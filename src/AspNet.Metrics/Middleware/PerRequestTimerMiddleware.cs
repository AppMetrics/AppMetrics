using System;
using System.Net;
using System.Threading.Tasks;
using Metrics;
using Metrics.Utils;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;

namespace AspNet.Metrics.Middleware
{
    public class PerRequestTimerMiddleware : MetricsMiddlewareBase
    {
        private const string TimerItemsKey = "__Mertics.PerRequestStartTime__";
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;

        public PerRequestTimerMiddleware(RequestDelegate next, MetricsOptions options, AspNetMetricsContext metricsContext)
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
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _next = next;
            _metricsContext = metricsContext;
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                context.Items[TimerItemsKey] = Clock.Default.Nanoseconds;

                await _next(context);

                if (context.HasMetricsCurrentRouteName() && context.Response.StatusCode != (int)HttpStatusCode.NotFound)
                {
                    var startTime = (long)context.Items[TimerItemsKey];
                    var elapsed = Clock.Default.Nanoseconds - startTime;
                    _metricsContext.Context.Timer(context.GetMetricsCurrentRouteName(), Unit.Requests).Record(elapsed, TimeUnit.Nanoseconds);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}