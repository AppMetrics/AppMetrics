// <copyright file="JsonHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health.Formatters.Json
{
    public class JsonHealthResponseWriter : IHealthResponseWriter
    {
        private readonly IHealthStatusSerializer _serializer;

        public JsonHealthResponseWriter(IHealthStatusSerializer serializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <inheritdoc />
        public string ContentType => "application/vnd.app.metrics.v1.health+json";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken))
        {
            var json = _serializer.Serialize(healthStatus);

            return context.Response.WriteAsync(json, token);
        }
    }
}