// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Extensions.Reporting.Console
{
    public class ConsoleReporter : IMetricReporter
    {
        private static readonly string GlobalName =
            $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";

        public ConsoleReporter(TimeSpan reportInterval)
            : this("Console Reporter", reportInterval)
        {
        }

        public ConsoleReporter(string name, TimeSpan reportInterval)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Name = name;
            ReportInterval = reportInterval;
        }

        public string Name { get; }

        public TimeSpan ReportInterval { get; }

        public void Dispose()
        {
        }

        public void EndMetricTypeReport(Type metricType)
        {
            WriteLine(metricType.HumanzeEndMetricType());
        }

        public void EndReport(IMetrics metrics)
        {
            WriteLine(string.Format(Environment.NewLine + "-- End {0} Report: {1} - {2} --" + Environment.NewLine,
                Name, GlobalName, metrics.Advanced.Clock.FormatTimestamp(metrics.Advanced.Clock.UtcDateTime)));
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            WriteLine(environmentInfo.Hummanize());
        }

        public void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> degradedChecks, IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            var passed = healthyChecks.ToList();
            var failed = unhealthyChecks.ToList();
            var degraded = degradedChecks.ToList();
            var isHealthy = !failed.Any() && !degraded.Any();
            var isUnhealthy = failed.Any();

            var status = "Degraded";

            if (isHealthy)
            {
                status = "Healthy";
            }

            if (isUnhealthy)
            {
                status = "Unhealthy";
            }


            WriteLine(string.Format(Environment.NewLine + $"\tHealth Status = {status}" + Environment.NewLine));

            WriteLine("\tPASSED CHECKS");

            passed.ForEach(c => WriteLine(c.Hummanize()));

            WriteLine("\tDEGRADED CHECKS");

            degraded.ForEach(c => WriteLine(c.Hummanize()));

            WriteLine("\tFAILED CHECKS");

            failed.ForEach(c => WriteLine(c.Hummanize()));
        }

        public void ReportMetric<T>(string name, MetricValueSource<T> valueSource, MetricTags globalTags)
        {
            WriteLine(valueSource.HumanzizeName(name));

            WriteLine(valueSource.Hummanize());
        }

        public void StartMetricTypeReport(Type metricType)
        {
            WriteLine(metricType.HumanzeStartMetricType());
        }

        public void StartReport(IMetrics metrics)
        {
            WriteLine(string.Format(Environment.NewLine + "-- Start {0} Report: {1} - {2} --" + Environment.NewLine,
                Name, GlobalName, metrics.Advanced.Clock.FormatTimestamp(metrics.Advanced.Clock.UtcDateTime)));
        }

        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}