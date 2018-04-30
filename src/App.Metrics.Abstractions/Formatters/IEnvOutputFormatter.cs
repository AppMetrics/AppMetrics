// <copyright file="IEnvOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Formatters
{
    public interface IEnvOutputFormatter
    {
        /// <summary>
        ///     Gets the <see cref="MetricsMediaTypeValue" /> of the formatter.
        /// </summary>
        MetricsMediaTypeValue MediaType { get; }

        /// <summary>
        ///     Writes the specified <see cref="EnvironmentInfo" /> to the given stream.
        /// </summary>
        /// <param name="output">The output stream of the formatted <see cref="EnvironmentInfo" /></param>
        /// .
        /// <param name="environmentInfo">The <see cref="EnvironmentInfo" /> being written.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /></param>
        /// <returns>A <see cref="Task" /> representing the asynchronous write operation.</returns>
        Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default);
    }
}