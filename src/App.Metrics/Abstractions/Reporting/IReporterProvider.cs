// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Filtering;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IReporterProvider
    {
        IFilterMetrics Filter { get; }

        IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory);
    }
}