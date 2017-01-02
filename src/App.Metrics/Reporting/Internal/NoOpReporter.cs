// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading;
using App.Metrics.Internal;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Reporting.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpReporter : IReporter
    {
        public void Dispose()
        {
        }

        public void RunReports(IMetrics context, CancellationToken token)
        {
        }
    }
}