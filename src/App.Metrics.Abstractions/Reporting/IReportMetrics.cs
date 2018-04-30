// <copyright file="IReportMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting
{
    public interface IReportMetrics
    {
        /// <summary>
        ///     Gets the <see cref="IFilterMetrics" /> to use for just this reporter provider.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter provider.
        /// </value>
        IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets <see cref="TimeSpan" /> interval to flush metrics values. Defaults to
        ///     <see cref="AppMetricsConstants.Reporting.DefaultFlushInterval" />.
        /// </summary>
        TimeSpan FlushInterval { get; set; }

        /// <summary>
        ///     Gets the <see cref="IMetricsOutputFormatter" /> used to format <see cref="MetricsDataValueSource" /> before
        ///     flushing. If not set the default <see cref="IMetricsOutputFormatter" /> will be used.
        /// </summary>
        IMetricsOutputFormatter Formatter { get; set; }

        /// <summary>
        ///     Flushes the current metrics snapshot using the configured output formatter.
        /// </summary>
        /// <param name="metricsData">The current snapshot of metrics.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if metrics were successfully flushed, false otherwise.</returns>
        Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default);
    }
}