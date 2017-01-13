// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Interfaces
{
    public interface IReporterProvider
    {
        IMetricsFilter Filter { get; }

        IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory);
    }
}