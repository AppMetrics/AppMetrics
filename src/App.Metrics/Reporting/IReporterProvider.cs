// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace App.Metrics.Reporting
{
    public interface IReporterProvider : IDisposable
    {
        IMetricsFilter Filter { get; }

        IReporterSettings Settings { get; }

        IMetricReporter CreateMetricReporter(string name);
    }
}