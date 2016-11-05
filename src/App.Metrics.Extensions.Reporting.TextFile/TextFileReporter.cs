// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Reporting;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public class TextFileReporter : IMetricReporter
    {
        private readonly string _file;
        private readonly StringReporter _stringReporter;
        private bool _disposed;

        public TextFileReporter(string file, TimeSpan interval)
            : this("Text File Reporter", file, interval)
        {
        }

        public TextFileReporter(string name, string file, TimeSpan interval)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            _file = file;
            _stringReporter = new StringReporter(name);
            ReportInterval = interval;
        }


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

            _disposed = true;
        }

        public TimeSpan ReportInterval { get; }

        public void EndMetricTypeReport(Type metricType)
        {
            _stringReporter.EndMetricTypeReport(metricType);
        }

        public void EndReport(IMetricsContext metricsContext)
        {
            _stringReporter.EndReport(metricsContext);

            try
            {
                var file = new FileInfo(_file);
                file.Directory.Create();
                File.WriteAllText(_file, _stringReporter.Result);
            }
            catch (Exception)
            {
                //TODO: AH - log
            }
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            _stringReporter.ReportEnvironment(environmentInfo);
        }

        public void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            _stringReporter.ReportHealth(healthyChecks, unhealthyChecks);
        }

        public void ReportMetric<T>(string name, MetricValueSource<T> valueSource, MetricTags globalTags)
        {
            _stringReporter.ReportMetric(name, valueSource, globalTags);
        }

        public void StartMetricTypeReport(Type metricType)
        {
            _stringReporter.StartMetricTypeReport(metricType);
        }

        public void StartReport(IMetricsContext metricsContext)
        {
            _stringReporter.StartReport(metricsContext);
        }
    }
}