using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Infrastructure;
using App.Metrics.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsJsonBuilder _jsonBuilder;
        private readonly EnvironmentInfoBuilder _environmentInfoBuilder;

        public MetricsEndpointMiddleware(RequestDelegate next,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricsJsonBuilder jsonBuilder, 
            EnvironmentInfoBuilder environmentInfoBuilder)
            : base(next, options, loggerFactory, metricsContext)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (environmentInfoBuilder == null)
            {
                throw new ArgumentNullException(nameof(environmentInfoBuilder));
            }

            _jsonBuilder = jsonBuilder;
            _environmentInfoBuilder = environmentInfoBuilder;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsEnabled && Options.MetricsEndpoint.HasValue && Options.MetricsEndpoint == context.Request.Path)
            {
                var environmentInfo = await _environmentInfoBuilder.BuildAsync();
                var json = _jsonBuilder.BuildJson(MetricsContext.Advanced.MetricsDataProvider.CurrentMetricsData, environmentInfo);

                await WriteResponseAsync(context, json, _jsonBuilder.MetricsMimeType);
                return;
            }

            await _next(context);
        }
    }
}