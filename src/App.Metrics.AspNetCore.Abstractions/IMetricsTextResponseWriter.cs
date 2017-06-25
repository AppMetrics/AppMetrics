// <copyright file="IMetricsTextResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

// ReSharper disable CheckNamespace
namespace App.Metrics.Middleware
    // ReSharper restore CheckNamespace
{
    public interface IMetricsTextResponseWriter
    {
        string ContentType { get; }

        Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken));
    }
}