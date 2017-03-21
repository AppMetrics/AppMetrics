// <copyright file="MetricsEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Serialization;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<AspNetMetricsOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string MetricsMimeType = "application/vnd.app.metrics.v1.metrics+json";
        private readonly RequestDelegate _next;
        private readonly IMetricDataSerializer _serializer;

        public MetricsEndpointMiddleware(
            RequestDelegate next,
            AspNetMetricsOptions aspNetOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricDataSerializer serializer)
            : base(next, aspNetOptions, loggerFactory, metrics)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _serializer = serializer;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.MetricsEndpointEnabled && Options.MetricsEndpoint.IsPresent() && Options.MetricsEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                var metricsData = Metrics.Snapshot.Get();

                var json = _serializer.Serialize(metricsData);

                await WriteResponseAsync(context, json, MetricsMimeType);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}