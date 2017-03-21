// <copyright file="NoOpReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using App.Metrics.Core.Internal;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Reporting.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpReporter : IReporter
    {
        /// <inheritdoc />
        public void Dispose() { }

        public void RunReports(IMetrics context, CancellationToken token) { }
    }
}