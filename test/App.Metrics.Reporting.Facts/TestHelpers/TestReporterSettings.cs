// <copyright file="TestReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.Facts.TestHelpers
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

        public Func<string, string, string> MetricNameFormatter { get; set; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }
    }
}