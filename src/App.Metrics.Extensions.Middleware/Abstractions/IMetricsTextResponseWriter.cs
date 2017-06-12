// <copyright file="IMetricsTextResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Extensions.Middleware.Abstractions
{
    public interface IMetricsTextResponseWriter
    {
        string ContentType { get; }

        Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken));
    }
}