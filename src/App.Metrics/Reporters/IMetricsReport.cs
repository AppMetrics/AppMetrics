// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public interface IMetricsReport : IHideObjectMembers, IDisposable
    {
        IMetricsFilter Filter { get; }

        Task RunReport(IMetricsContext metricsContext, CancellationToken token);
    }
}