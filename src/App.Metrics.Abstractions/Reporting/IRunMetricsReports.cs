// <copyright file="IRunMetricsReports.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    public interface IRunMetricsReports
    {
        /// <summary>
        ///     Returns an <see cref="IEnumerable{Task}" /> which flush the current snapshot of metrics via each configured
        ///     <see cref="IReportMetrics" />.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to use for each reporting task.</param>
        /// <returns>Tasks to flush the current snapshot of metrics via each configured <see cref="IReportMetrics" />.</returns>
        IEnumerable<Task> RunAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns an <see cref="Task" /> which flushes the current snapshot of metrics via the specified
        ///     <see cref="IReportMetrics" /> type. The <see cref="IReportMetrics" /> must be configured on the
        ///     <see cref="IMetricsBuilder" /> otherwise throws an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="TMetricReporter">The typeof of reporter used to flush the current snapshot of metrics.</typeparam>
        /// <param name="cancellationToken">The cancellation token to use for the reporting task.</param>
        /// <returns>Tasks to flush the current snapshot of metrics via each configured <see cref="IReportMetrics" />.</returns>
        Task RunAsync<TMetricReporter>(CancellationToken cancellationToken = default)
            where TMetricReporter : IReportMetrics;
    }
}