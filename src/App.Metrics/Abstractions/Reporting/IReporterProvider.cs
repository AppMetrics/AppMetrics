// <copyright file="IReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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