// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Reporting
{
    public class ConsoleReporter : IMetricReporter
    {
        public ConsoleReporter(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }

        public void Dispose()
        {
        }

        public void EndMetricTypeReport(Type metricType)
        {
            WriteLine(metricType.HumanzeEndMetricType());
        }

        public void EndReport(IMetricsContext metricsContext)
        {
            WriteLine(string.Format(Environment.NewLine + "-- End {0} Report: {1} - {2} --" + Environment.NewLine,
                Name, metricsContext.ContextName, metricsContext.Advanced.Clock.FormatTimestamp(metricsContext.Advanced.Clock.UtcDateTime)));
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            WriteLine(environmentInfo.Hummanize());
        }

        public void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            var passed = healthyChecks.ToList();
            var failed = unhealthyChecks.ToList();
            var isHealthy = !failed.Any();

            WriteLine(string.Format(Environment.NewLine + "\tIs Healthy = " + (isHealthy ? "Yes" : "No") + Environment.NewLine));

            WriteLine("\tPASSED CHECKS");

            passed.ForEach(c => WriteLine(c.Hummanize()));

            WriteLine("\tFAILED CHECKS");

            failed.ForEach(c => WriteLine(c.Hummanize()));
        }

        public void ReportMetric<T>(string name, MetricValueSource<T> valueSource)
        {
            WriteLine(valueSource.HumanzizeName(name));

            WriteLine(valueSource.Hummanize());
        }

        public void StartMetricTypeReport(Type metricType)
        {
            WriteLine(metricType.HumanzeStartMetricType());
        }

        public void StartReport(IMetricsContext metricsContext)
        {
            WriteLine(string.Format(Environment.NewLine + "-- Start {0} Report: {1} - {2} --" + Environment.NewLine,
                Name, metricsContext.ContextName, metricsContext.Advanced.Clock.FormatTimestamp(metricsContext.Advanced.Clock.UtcDateTime)));
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}