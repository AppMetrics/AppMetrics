// <copyright file="IMetricsResponseWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore
{
    public interface IMetricsResponseWriter
    {
        /// <summary>
        ///     Writes the specified <see cref="MetricsDataValueSource" /> to the <see cref="HttpContext" /> response.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext" /> for which the snapshot will be written.</param>
        /// <param name="metricsData">The metrics snapshot to write.</param>
        /// <param name="token">The <see cref="CancellationToken" /></param>
        /// <returns>A <see cref="Task" /> which writes the given <see cref="MetricsDataValueSource" /> to the HTTP reponse.</returns>
        Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default);
    }
}