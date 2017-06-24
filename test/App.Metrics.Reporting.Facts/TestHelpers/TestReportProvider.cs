// <copyright file="TestReportProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public class TestReportProvider : IReporterProvider
    {
        private readonly bool _pass;
        private readonly TimeSpan _reportInterval;
        private readonly Exception _throwEx;

        public TestReportProvider(bool pass, TimeSpan reportInterval, Exception throwEx = null)
        {
            _pass = throwEx == null && pass;
            _reportInterval = reportInterval;
            _throwEx = throwEx;
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public IFilterMetrics Filter { get; }
        // ReSharper restore UnassignedGetOnlyAutoProperty

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory) { return new TestMetricReporter(_pass, _reportInterval, _throwEx); }
    }
}