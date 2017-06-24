// <copyright file="IReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting
{
    public interface IReporterProvider
    {
        IFilterMetrics Filter { get; }

        IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory);
    }
}