// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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