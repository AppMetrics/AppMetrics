// <copyright file="NoOpHealthStatusResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core.Internal;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Extensions.Middleware.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal class NoOpHealthStatusResponseWriter : IHealthResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken))
        {
            return context.Response.WriteAsync("No formatter has been registered. See App.Metrics.Formatters.Json for example.", token);
        }
    }
}