using System;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Reporting;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestReporterSettings : IReporterSettings
    {
        public TestReporterSettings()
        {
            ReportInterval = TimeSpan.FromSeconds(5);
            MetricNameFormatter = (metricContext, metricName) => metricContext.IsMissing()
                ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
                : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();
        }

        /// <inheritdoc />
        public MetricValueDataKeys DataKeys { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        public Func<string, string, string> MetricNameFormatter { get; set; }
    }
}