using System;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly ICounter _activeRequests;

        public ActiveRequestCounterEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _activeRequests = metricsContext.GetWebApplicationContext()
                .Counter("Active Requests", Unit.Custom("ActiveRequests"));
        }

        public async Task Invoke(HttpContext context)
        {
            if (PerformMetric(context))
            {
                _activeRequests.Increment();

                await Next(context);

                _activeRequests.Decrement();
            }
            else
            {
                await Next(context);
            }
        }
    }
}