// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Reporting
{
    internal sealed class NullReporter : IReporter
    {
        public void Dispose()
        {
        }

        public Task RunReportsAsync(IMetrics context, CancellationToken token)
        {
            return AppMetricsTaskCache.EmptyTask;
        }
    }
}