// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using App.Metrics.Serialization.Interfaces;

namespace App.Metrics.Extensions.Middleware.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
    {
        private const string MetricsMimeType = "application/vnd.app.metrics.v1.metrics+json";
        private readonly RequestDelegate _next;
        private readonly IMetricDataSerializer _serializer;

        public MetricsEndpointMiddleware(RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricDataSerializer serializer)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _serializer = serializer;
            _next = next;
            _serializer = serializer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Options.MetricsEndpointEnabled && Options.MetricsEndpoint.IsPresent() && Options.MetricsEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var metricsData = Metrics.Advanced.Data.ReadData();

                var json = _serializer.Serialize(metricsData);

                await WriteResponseAsync(context, json, MetricsMimeType);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}