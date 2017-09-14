// <copyright file="NoOpMetricsReportRunner.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal class NoOpMetricsReportRunner : IRunMetricsReports
    {
        /// <inheritdoc />
        public IEnumerable<Task> RunAllAsync(CancellationToken cancellationToken = default) { return Enumerable.Empty<Task>(); }
    }
}