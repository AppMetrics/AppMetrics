// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Serialization.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        private const string MetricsMimeType = "application/vnd.app.metrics.v1.metrics+json";
        private readonly IMetricDataSerializer _serializer;

        public MetricsEndpointMiddleware(
            OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricDataSerializer serializer)
            : base(owinOptions, loggerFactory, metrics)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _serializer = serializer;
            _serializer = serializer;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var requestPath = environment["owin.RequestPath"] as string;

            if (Options.MetricsEndpointEnabled && Options.MetricsEndpoint.IsPresent() && Options.MetricsEndpoint == requestPath)
            {
                Logger.MiddlewareExecuting(GetType());

                var metricsData = Metrics.Advanced.Data.ReadData();

                var json = _serializer.Serialize(metricsData);

                await WriteResponseAsync(environment, json, MetricsMimeType);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(environment);
        }
    }
}