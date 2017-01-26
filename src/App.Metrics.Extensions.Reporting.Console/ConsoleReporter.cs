// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Core.Abstractions;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Tagging;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Console
{
    public class ConsoleReporter : IMetricReporter
    {
        private static readonly string GlobalName =
            $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";

        private readonly ILogger<ConsoleReporter> _logger;

        public ConsoleReporter(TimeSpan reportInterval, ILoggerFactory loggerFactory)
            : this(typeof(ConsoleReporter).Name, reportInterval, loggerFactory) { }

        public ConsoleReporter(string name, TimeSpan reportInterval, ILoggerFactory loggerFactory)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            Name = name;
            ReportInterval = reportInterval;

            _logger = loggerFactory.CreateLogger<ConsoleReporter>();
        }

        public string Name { get; }

        public TimeSpan ReportInterval { get; }

        public void Dispose() { _logger.LogDebug("Console Reporter Disposed"); }

        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            _logger.LogDebug("Ending Console Report Run");

            WriteLine(
                string.Format(
                    Environment.NewLine + "-- End {0} Report: {1} - {2} --" + Environment.NewLine,
                    Name,
                    GlobalName,
                    metrics.Clock.FormatTimestamp(metrics.Clock.UtcDateTime)));

            return AppMetricsTaskCache.SuccessTask;
        }

        public void EndMetricTypeReport(Type metricType) { }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            WriteLine(typeof(EnvironmentInfo).HumanzeStartMetricType());
            WriteLine(environmentInfo.Hummanize());
            WriteLine(typeof(EnvironmentInfo).HumanzeEndMetricType());
        }

        public void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            WriteLine(typeof(HealthStatus).HumanzeStartMetricType());
            _logger.LogDebug("Writing Health Checks for Console");

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

            _logger.LogDebug("Writing Health Checks for Console");
            WriteLine(typeof(HealthStatus).HumanzeEndMetricType());
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            _logger.LogDebug("Writing Metric {T} for Console", typeof(T));

            WriteLine(valueSource.HumanzizeName(context));

            WriteLine(valueSource.Hummanize());

            _logger.LogDebug("Writing Metric {T} for Console", typeof(T));
        }

        public void StartReportRun(IMetrics metrics)
        {
            _logger.LogDebug("Starting Console Report Run");

            WriteLine(
                string.Format(
                    Environment.NewLine + "-- Start {0} Report: {1} - {2} --" + Environment.NewLine,
                    Name,
                    GlobalName,
                    metrics.Clock.FormatTimestamp(metrics.Clock.UtcDateTime)));
        }

        public void WriteLine(string message) { System.Console.WriteLine(message); }

        private static string CleanName(string name) { return name.Replace('.', '_'); }
    }
}