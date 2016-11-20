// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Reporting.Interfaces;
using App.Metrics.Scheduling.Interfaces;

namespace App.Metrics.Reporting.Internal
{
    internal sealed class NoOpReportFactory : IReportFactory
    {
        public void AddProvider(IReporterProvider provider)
        {
        }

        public IReporter CreateReporter(IScheduler scheduler)
        {
            return new NoOpReporter();
        }

        public IReporter CreateReporter()
        {
            return new NoOpReporter();
        }

        public void Dispose()
        {
        }
    }
}