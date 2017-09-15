// <copyright file="NoOpMetricsReportRunner.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Internal;
#endif
using App.Metrics.Reporting;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal class NoOpMetricsReportRunner : IRunMetricsReports
    {
        /// <inheritdoc />
        public IEnumerable<Task> RunAllAsync(CancellationToken cancellationToken = default) { return Enumerable.Empty<Task>(); }

        /// <inheritdoc />
        public Task RunAsync<TMetricReporter>(CancellationToken cancellationToken = default)
            where TMetricReporter : IReportMetrics
        {
#if !NETSTANDARD1_6
            return AppMetricsTaskHelper.CompletedTask();
#else
            return Task.CompletedTask;
#endif
        }
    }
}