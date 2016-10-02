using System;
using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics.Middleware
{
    public class PerRequestTimerMiddleware : MetricsMiddlewareBase
    {
        private const string TimerItemsKey = "__Mertics.PerRequestStartTime__";
        private readonly AspNetMetricsContext _metricsContext;
        private readonly RequestDelegate _next;

        public PerRequestTimerMiddleware(RequestDelegate next, AspNetMetricsOptions options, AspNetMetricsContext metricsContext)
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
                context.Items[TimerItemsKey] = _metricsContext.Clock.Nanoseconds;

                await _next(context);

                if (context.HasMetricsCurrentRouteName() && context.Response.StatusCode != (int)HttpStatusCode.NotFound)
                {
                    var clientId = context.OAuthClientId();

                    var startTime = (long)context.Items[TimerItemsKey];
                    var elapsed = _metricsContext.Clock.Nanoseconds - startTime;

                    _metricsContext.Context.GetWebApplicationContext()
                        .Timer(context.GetMetricsCurrentRouteName(), Unit.Requests)
                        .Record(elapsed, TimeUnit.Nanoseconds, clientId.IsPresent() ? clientId : null);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}