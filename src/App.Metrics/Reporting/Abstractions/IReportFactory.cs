// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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