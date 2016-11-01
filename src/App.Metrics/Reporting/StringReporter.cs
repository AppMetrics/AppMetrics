using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Reporting
{
    public class StringReporter : IMetricReporter
    {
        private StringBuilder _buffer;
        private bool _disposed;

        public StringReporter()
        {
            _buffer = new StringBuilder();
        }

        //TODO: AH - is name required?
        public string Name { get; } = "Test";

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

        public string Result => _buffer.ToString();

        public void EndMetricTypeReport(Type metricType)
        {
            _buffer.WriteEndMetricType(metricType);
        }

        public void EndReport(IMetricsContext metricsContext)
        {
            _buffer.WriteMetricEndReport(Name, metricsContext.ContextName,
                metricsContext.Advanced.Clock.FormatTimestamp(metricsContext.Advanced.Clock.UtcDateTime));
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            _buffer.WriteEnvironmentInfo(environmentInfo);
        }

        public void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            var passed = healthyChecks.ToList();
            var failed = unhealthyChecks.ToList();

            _buffer.WriteHealthStatus(!failed.Any());

            _buffer.WritePassedHealthChecksHeader();
            ;
            passed.ForEach(c => _buffer.WriteHealthCheckResult(c));

            _buffer.WriteFailedHealthChecksHeader();

            failed.ForEach(c => _buffer.WriteHealthCheckResult(c));
        }

        public void ReportMetric<T>(string name, MetricValueSource<T> valueSource)
        {
            _buffer.WriteMetricName(name, valueSource);
            _buffer.WriteMetricValue(valueSource);
        }

        public void StartMetricTypeReport(Type metricType)
        {
            _buffer.WriteStartMetricType(metricType);
        }

        public void StartReport(IMetricsContext metricsContext)
        {
            _buffer.WriteMetricStartReport(Name, metricsContext.ContextName,
                metricsContext.Advanced.Clock.FormatTimestamp(metricsContext.Advanced.Clock.UtcDateTime));
        }
    }
}