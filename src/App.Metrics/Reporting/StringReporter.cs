// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Internal;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Reporting
{
    public sealed class StringReporter : IMetricReporter
    {
        private static readonly List<string> MetricTypesReported = new List<string>();
        private StringBuilder _buffer;
        private bool _disposed;

        public StringReporter()
            : this(typeof(StringReporter).Name) { }

        public StringReporter(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            _buffer = new StringBuilder();
        }

        ~StringReporter() { Dispose(false); }

        public string Name { get; }

        public TimeSpan ReportInterval { get; } = TimeSpan.FromSeconds(5);

        public string Result => _buffer?.ToString();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    if (_buffer != null)
                    {
                        _buffer.Clear();
                        _buffer = null;
                    }
                }
            }

            _disposed = true;
        }

        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            _buffer.WriteMetricEndReport(
                Name,
                metrics.Advanced.Clock.FormatTimestamp(metrics.Advanced.Clock.UtcDateTime));

            return AppMetricsTaskCache.SuccessTask;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            _buffer.WriteStartMetricType(typeof(EnvironmentInfo));
            _buffer.WriteEnvironmentInfo(environmentInfo);
        }

        public void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            var passed = healthyChecks.ToList();
            var failed = unhealthyChecks.ToList();
            var degraded = degradedChecks.ToList();

            var status = Constants.Health.DegradedStatusDisplay;

            if (!degraded.Any() && !failed.Any())
            {
                status = Constants.Health.HealthyStatusDisplay;
            }

            if (failed.Any())
            {
                status = Constants.Health.UnhealthyStatusDisplay;
            }

            _buffer.WriteHealthStatus(status);

            _buffer.WritePassedHealthChecksHeader();

            passed.ForEach(c => _buffer.WriteHealthCheckResult(c));

            _buffer.WriteDegradedHealthChecksHeader();

            degraded.ForEach(c => _buffer.WriteHealthCheckResult(c));

            _buffer.WriteFailedHealthChecksHeader();

            failed.ForEach(c => _buffer.WriteHealthCheckResult(c));
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            WriteStartMetricType<T>(context);
            _buffer.WriteMetricName(valueSource);
            _buffer.WriteMetricValue(valueSource);
        }

        public void StartReportRun(IMetrics metrics)
        {
            _buffer.WriteMetricStartReport(
                Name,
                metrics.Advanced.Clock.FormatTimestamp(metrics.Advanced.Clock.UtcDateTime));
        }

        private void WriteStartMetricType<T>(string context)
        {
            var key = $"{context}_{typeof(T).Name}";
            if (MetricTypesReported.Contains(key))
            {
                return;
            }

            _buffer.WriteStartMetricType(typeof(T), context);

            MetricTypesReported.Add(key);
        }
    }
}