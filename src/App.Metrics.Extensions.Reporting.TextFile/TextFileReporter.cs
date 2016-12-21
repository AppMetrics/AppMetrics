// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;
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
            : this("Text File Reporter", file, interval, loggerFactory)
        {
        }

        public TextFileReporter(string name, string file, TimeSpan interval, ILoggerFactory loggerFactory)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            _file = file;
            _logger = loggerFactory.CreateLogger<TextFileReporter>();
            _stringReporter = new StringReporter(name);
            ReportInterval = interval;
        }

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

            _logger.LogDebug("TextFile Reporter Disposed");

            _disposed = true;
        }

        public void EndMetricTypeReport(Type metricType)
        {
            _stringReporter.EndMetricTypeReport(metricType);
        }

        public void EndReport(IMetrics metrics)
        {
            _logger.LogDebug("Ending TextFile Report Run");

            _stringReporter.EndReport(metrics);

            var file = new FileInfo(_file);
            file.Directory.Create();
            File.WriteAllText(_file, _stringReporter.Result);
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            _stringReporter.ReportEnvironment(environmentInfo);
        }

        public void ReportHealth(GlobalMetricTags globalMetrics,
            IEnumerable<HealthCheck.Result> healthyChecks, 
            IEnumerable<HealthCheck.Result> degradedChecks, 
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            _logger.LogDebug("Writing Health Checks for TextFile");

            _stringReporter.ReportHealth(globalMetrics, healthyChecks, degradedChecks, unhealthyChecks);

            _logger.LogDebug("Writing Health Checks for TextFile");

        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            _logger.LogDebug("Writing Metric {T} for TextFile", typeof(T));

            _stringReporter.ReportMetric(context, valueSource);

            _logger.LogDebug("Writing Metric {T} for TextFile", typeof(T));
        }

        public void StartMetricTypeReport(Type metricType)
        {
            _stringReporter.StartMetricTypeReport(metricType);
        }

        public void StartReport(IMetrics metrics)
        {
            _logger.LogDebug("Starting TextFile Report Run");

            _stringReporter.StartReport(metrics);
        }
    }
}