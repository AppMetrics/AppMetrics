// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.MetricData;
using App.Metrics.Reporters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly StringReport _stringReport;

        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next,
            IMetricsFilter metricsFilter,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _stringReport = new StringReport(loggerFactory, metricsContext, metricsFilter);
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsTextEnabled && Options.MetricsTextEndpoint.HasValue && Options.MetricsTextEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var content = await _stringReport.RenderMetrics(MetricsContext);

                await WriteResponseAsync(context, content, "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}