// <copyright file="IMetricsOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters
{
    public interface IMetricsOutputFormatter
    {
        /// <summary>
        ///     Gets the <see cref="MetricsMediaTypeValue" /> of the formatter.
        /// </summary>
        MetricsMediaTypeValue MediaType { get; }

        MetricFields MetricFields { get; set; }

        /// <summary>
        ///     Writes the specified <see cref="MetricsDataValueSource" /> to the given stream.
        /// </summary>
        /// <param name="output">The output stream of the formatted <see cref="MetricsDataValueSource" /></param>
        /// .
        /// <param name="metricsData">The <see cref="MetricsDataValueSource" /> being written.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
        /// <returns>A <see cref="Task" /> representing the asynchronous write operation.</returns>
        Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default);
    }
}