// <copyright file="IHealthResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;

// ReSharper disable CheckNamespace
namespace App.Metrics.AspNetCore.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthResponseWriter
    {
        Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default);
    }
}