// <copyright file="TestReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting;

namespace App.Metrics.Facts.TestHelpers
{
    public class TestReporter : IReportMetrics
    {
        private readonly bool _pass;
        private readonly Exception _throwEx;
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsTextOutputFormatter();

        public TestReporter()
        {
            Formatter = _defaultMetricsOutputFormatter;
        }

        public TestReporter(bool pass = true, Exception throwEx = null)
        {
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
            FlushInterval = TimeSpan.FromMilliseconds(10);
            Filter = new NullMetricsFilter();
        }

        public TestReporter(TimeSpan interval, bool pass = true, Exception throwEx = null)
        {
            FlushInterval = interval;
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
            Filter = new NullMetricsFilter();
        }

        public IFilterMetrics Filter { get; set; }

        public TimeSpan FlushInterval { get; set; }

        public IMetricsOutputFormatter Formatter { get; set; }

        public Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            if (_throwEx != null)
            {
                throw _throwEx;
            }

            return Task.FromResult(_pass);
        }
    }
}