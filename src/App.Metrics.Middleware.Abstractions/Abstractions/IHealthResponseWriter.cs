// <copyright file="IHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;

// ReSharper disable CheckNamespace
namespace App.Metrics.Middleware
    // ReSharper restore CheckNamespace
{
    public interface IHealthResponseWriter
    {
        string ContentType { get; }

        Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken));
    }
}