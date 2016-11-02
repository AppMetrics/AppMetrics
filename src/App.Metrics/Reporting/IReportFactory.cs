// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Scheduling;

namespace App.Metrics.Reporting
{
    public interface IReportFactory : IDisposable
    {
        void AddProvider(IReporterProvider provider);

        IReporter CreateReporter(string name);

        IReporter CreateReporter(string name, IScheduler scheduler);
    }
}