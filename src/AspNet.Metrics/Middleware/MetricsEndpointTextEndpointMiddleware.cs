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
        private readonly IMetricsFilter _metricsFilter;
        private readonly DefaultReportGenerator _reportGenerator;
        private readonly StringReporter _stringReporter;

        public MetricsEndpointTextEndpointMiddleware(RequestDelegate next,
            IMetricsFilter metricsFilter,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext)
            : base(next, options, loggerFactory, metricsContext)
        {
            _metricsFilter = metricsFilter;
            _stringReporter = new StringReporter();
            _reportGenerator = new DefaultReportGenerator();
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsTextEndpointEnabled && Options.MetricsTextEndpoint.HasValue && Options.MetricsTextEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                //DEVNOTE: MetricsTags.None as it's not very useful here are the moment, if filtering was provided via the url it could be useful
                // this would also allow filtering when pulling metrics rather than pushing
                await _reportGenerator.Generate(_stringReporter, MetricsContext, _metricsFilter, MetricTags.None, context.RequestAborted);

                await WriteResponseAsync(context, _stringReporter.Result, "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}