// <copyright file="JsonMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Middleware.Formatters.Json.Abstractions.Serialization;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Middleware.Formatters.Json
{
    public class JsonMetricsResponseWriter : IMetricsResponseWriter
    {
        private readonly IMetricDataSerializer _serializer;

        public JsonMetricsResponseWriter(IMetricDataSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc />
        public string ContentType => "application/vnd.app.metrics.v1.metrics+json";

        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var json = _serializer.Serialize(metricsData);

            return context.Response.WriteAsync(json, token);
        }
    }
}