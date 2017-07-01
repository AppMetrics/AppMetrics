// <copyright file="TestMetricReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core.Tagging;
using App.Metrics.Infrastructure;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public class TestMetricReporter : IMetricReporter
    {
        private readonly bool _pass;
        private readonly Exception _throwEx;

        public TestMetricReporter(bool pass, TimeSpan reportInterval, Exception throwEx = null)
        {
            _pass = pass;
            _throwEx = throwEx;
            ReportInterval = reportInterval;
        }

        public string Name { get; } = "Test Reporter";

        public TimeSpan ReportInterval { get; }

        public void Dispose() { }

        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            if (_throwEx != null)
            {
                throw _throwEx;
            }

            return Task.FromResult(_pass);
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo) { }

        public void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource) { }

        public void StartReportRun(IMetrics metrics) { }
    }
}