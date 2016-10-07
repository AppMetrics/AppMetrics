using System;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class RequestTimerMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string TimerItemsKey = "__Mertics.RequestTimer__";
        private readonly ITimer _requestTimer;

        public RequestTimerMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _requestTimer = MetricsContext.GetWebApplicationContext()
                .Timer("Web Requests", Unit.Requests);
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                context.Items[TimerItemsKey] = _requestTimer.NewContext();

                await Next(context);

                var timer = context.Items[TimerItemsKey];
                using (timer as IDisposable)
                {
                }
                context.Items.Remove(TimerItemsKey);
            }
            else
            {
                await Next(context);
            }
        }
    }
}