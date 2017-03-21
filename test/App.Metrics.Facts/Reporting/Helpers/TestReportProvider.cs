// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Reporting.Helpers
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

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return new TestMetricReporter(_pass, _reportInterval, _throwEx);
        }
    }
}