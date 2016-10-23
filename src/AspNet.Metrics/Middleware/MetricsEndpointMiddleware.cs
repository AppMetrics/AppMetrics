// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Infrastructure;
using App.Metrics.Json;
using App.Metrics.MetricData;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private readonly EnvironmentInfoBuilder _environmentInfoBuilder;
        private readonly IMetricsJsonBuilder _jsonBuilder;
        private readonly RequestDelegate _next;
        private readonly IMetricsFilter _metricsFilter;

        public MetricsEndpointMiddleware(RequestDelegate next,
            IMetricsFilter metricsFilter,
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
            if (metricsFilter == null)
            {
                throw new ArgumentNullException(nameof(metricsFilter));
            }

            if (environmentInfoBuilder == null)
            {
                throw new ArgumentNullException(nameof(environmentInfoBuilder));
            }

            _jsonBuilder = jsonBuilder;
            _environmentInfoBuilder = environmentInfoBuilder;
            _next = next;
            _metricsFilter = metricsFilter;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsEnabled && Options.MetricsEndpoint.HasValue && Options.MetricsEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var json = await _jsonBuilder.BuildJsonAsync(MetricsContext, _metricsFilter);

                await WriteResponseAsync(context, json, _jsonBuilder.MetricsMimeType);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}