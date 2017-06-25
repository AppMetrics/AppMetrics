// <copyright file="NoOpMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Middleware.Internal
{
    [ExcludeFromCodeCoverage]
    internal class NoOpMetricsResponseWriter : IMetricsResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            return context.Response.WriteAsync("No formatter has been registered. See App.Metrics.Formatters.Json for example.", token);
        }
    }
}