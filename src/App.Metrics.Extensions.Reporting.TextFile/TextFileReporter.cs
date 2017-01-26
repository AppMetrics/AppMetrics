// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Core.Abstractions;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting;
using App.Metrics.Tagging;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public class TextFileReporter : IMetricReporter
    {
        private readonly string _file;
        private readonly ILogger<TextFileReporter> _logger;
        private readonly StringReporter _stringReporter;
        private bool _disposed;

        public TextFileReporter(string file, TimeSpan interval, ILoggerFactory loggerFactory)
            : this(typeof(TextFileReporter).Name, file, interval, loggerFactory) { }

        public TextFileReporter(string name, string file, TimeSpan interval, ILoggerFactory loggerFactory)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _file = file;
            _logger = loggerFactory.CreateLogger<TextFileReporter>();
            _stringReporter = new StringReporter(name);
            Name = name;
            ReportInterval = interval;
        }

        public string Name { get; }

        public TimeSpan ReportInterval { get; }

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

                    _stringReporter?.Dispose();
                }
            }

            _logger.LogDebug($"{Name} Disposed");

            _disposed = true;
        }

        public async Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            await _stringReporter.EndAndFlushReportRunAsync(metrics);

            _logger.LogDebug($"End {Name} Run");

            var file = new FileInfo(_file);
            file.Directory.Create();
            File.WriteAllText(_file, _stringReporter.Result);

            return true;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo) { _stringReporter.ReportEnvironment(environmentInfo); }

        public void ReportHealth(
            GlobalMetricTags globalMetrics,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            _logger.LogDebug($"Writing Health Checks for {Name}");

            _stringReporter.ReportHealth(globalMetrics, healthyChecks, degradedChecks, unhealthyChecks);

            _logger.LogDebug($"Writing Health Checks for {Name}");
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            _logger.LogDebug($"Start Writing Metric {typeof(T)} for {Name}");

            _stringReporter.ReportMetric(context, valueSource);

            _logger.LogDebug($"End Writing Metric {typeof(T)} for {Name}");
        }

        public void StartReportRun(IMetrics metrics)
        {
            _logger.LogDebug($"Starting {Name} Run");

            _stringReporter.StartReportRun(metrics);
        }
    }
}