// <copyright file="NoOpReportFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Reporting;
using App.Metrics.Scheduling;

namespace App.Metrics.Core.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal sealed class NoOpReportFactory : IReportFactory
    {
        public void AddProvider(IReporterProvider provider) { }

        public IReporter CreateReporter(IScheduler scheduler) { return new NoOpReporter(); }

        public IReporter CreateReporter() { return new NoOpReporter(); }
    }
}