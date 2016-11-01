// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly DefaultReportGenerator _reportGenerator;
        private readonly StringReporter _stringReporter;

        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next,
            IMetricsFilter metricsFilter,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _stringReporter = new StringReporter();
            _reportGenerator = new DefaultReportGenerator();
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsTextEndpointEnabled && Options.MetricsTextEndpoint.HasValue && Options.MetricsTextEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                await _reportGenerator.Generate(_stringReporter, MetricsContext, context.RequestAborted);

                await WriteResponseAsync(context, _stringReporter.Result, "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}