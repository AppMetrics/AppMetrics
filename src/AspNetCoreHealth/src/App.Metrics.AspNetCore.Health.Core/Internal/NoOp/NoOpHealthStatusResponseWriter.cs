// <copyright file="NoOpHealthStatusResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health.Core.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public class NoOpHealthStatusResponseWriter : IHealthResponseWriter
    {
        public string ContentType => "text/plain";

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default)
        {
            return context.Response.WriteAsync(
                "No formatter has been registered. See App.Metrics.Formatters.Ascii & App.Metrics.Formatters.Json for example.",
                token);
        }
    }
}