// <copyright file="IReportFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Reporting;
using App.Metrics.Scheduling.Abstractions;

namespace App.Metrics.Reporting.Abstractions
{
    public interface IReportFactory
    {
        void AddProvider(IReporterProvider provider);

        IReporter CreateReporter(IScheduler scheduler);

        IReporter CreateReporter();
    }
}