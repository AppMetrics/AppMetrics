// <copyright file="NoOpMetricsReportScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using App.Metrics.Reporting;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal class NoOpMetricsReportScheduler : IScheduleMetricsReporting
    {
        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public void ScheduleAll(CancellationToken cancellationToken = default) { }

        /// <inheritdoc />
        public void ScheduleAll(TimeSpan interval, CancellationToken cancellationToken = default) { }
    }
}