// <copyright file="NoOpReportFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Reporting;
using App.Metrics.Core.Internal;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Scheduling.Abstractions;

namespace App.Metrics.Reporting.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpReportFactory : IReportFactory
    {
        public void AddProvider(IReporterProvider provider) { }

        public IReporter CreateReporter(IScheduler scheduler) { return new NoOpReporter(); }

        public IReporter CreateReporter() { return new NoOpReporter(); }
    }
}