// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string MetricsMimeType = "application/vnd.app.metrics.v1.metrics+json";
        private readonly IMetricsFilter _metricsFilter;
        private readonly RequestDelegate _next;
        private readonly IMetricDataSerializer _serializer;

        public MetricsEndpointMiddleware(RequestDelegate next,
            IMetricsFilter metricsFilter,
            IOptions<AspNetMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricDataSerializer serializer)
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
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _serializer = serializer;
            _next = next;
            _metricsFilter = metricsFilter;
            _serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsEndpointEnabled && Options.MetricsEndpoint.HasValue && Options.MetricsEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var metricsData = await MetricsContext.Advanced.DataManager.GetMetricsDataAsync();

                if (_metricsFilter != null)
                {
                    //TODO: AH - shouldn't have to apply the filter here
                    metricsData = metricsData.Filter(_metricsFilter);
                }

                var json = _serializer.Serialize(metricsData);

                await WriteResponseAsync(context, json, MetricsMimeType);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}