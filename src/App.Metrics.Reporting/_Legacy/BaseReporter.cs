// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting._Legacy
{
    public abstract class BaseReporter : IMetricReporter
    {
        protected readonly ILogger Logger;
        private bool _disposed = false;
        private CancellationToken _token;

        protected BaseReporter(ILoggerFactory loggerFactory,
            IClock clock,
            IMetricsFilter filter)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (clock == null)
            {
                throw new ArgumentNullException(nameof(clock));
            }

            Logger = loggerFactory.CreateLogger(GetType());
            Filter = filter;
            Clock = clock;
        }

        ~BaseReporter()
        {
            Dispose(false);
        }

        public IMetricsFilter Filter { get; }

        protected IClock Clock { get; }

        protected DateTime CurrentContextTimestamp { get; private set; }

        protected DateTime ReportTimestamp { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task RunReport(IMetricsContext metricsContext, CancellationToken token)
        {
            var dataProvider = metricsContext.Advanced.DataManager;

            if (Filter != default(IMetricsFilter))
            {
                dataProvider = dataProvider.WithFilter(Filter);
            }

            var metricsData = await dataProvider.GetMetricsDataAsync();

            _token = token;

            ReportTimestamp = metricsContext.Advanced.Clock.UtcDateTime;

            StartReport(metricsData.ContextName);

            ReportContext(metricsData, Enumerable.Empty<string>());

            await ReportHealthStatus(metricsContext.Advanced.HealthCheckManager);

            EndReport(metricsData.ContextName);
        }

        public abstract void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags);
        public abstract void ReportGauge(string name, double value, Unit unit, MetricTags tags);
        public abstract void ReportHealth(HealthStatus status);
        public abstract void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags);
        public abstract void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags);
        public abstract void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }

        public virtual void EndContext(string contextName)
        {
        }

        public virtual void EndMetricGroup(string metricName)
        {
        }

        public virtual void EndReport(string contextName)
        {
        }

        public virtual string FormatContextName(IEnumerable<string> contextStack, string contextName)
        {
            var stack = string.Join(" - ", contextStack);
            if (stack.Length == 0)
            {
                return contextName;
            }
            return string.Concat(stack, " - ", contextName);
        }

        public virtual string FormatMetricName<T>(string context, MetricValueSource<T> metric)
        {
            return string.Concat("[", context, "] ", metric.Name);
        }

        public virtual void ReportEnvironment(string name, IEnumerable<EnvironmentInfoEntry> environment)
        {
        }

        public virtual void StartContext(string contextName)
        {
        }

        public virtual void StartMetricGroup(string metricName)
        {
        }

        public virtual void StartReport(string contextName)
        {
        }

        private void ReportContext(MetricsData data, IEnumerable<string> contextStack)
        {
            CurrentContextTimestamp = data.Timestamp;
            var contextName = FormatContextName(contextStack, data.ContextName);
            ReportEnvironment(contextName, data.Environment.Entries);

            foreach (var group in data.Groups)
            {
                StartContext(group.GroupName);

                ReportSection("Gauges", group.Gauges, g => ReportGauge(FormatMetricName(group.GroupName, g),
                g.Value, g.Unit, g.Tags));

                ReportSection("Counters", group.Counters, c => ReportCounter(FormatMetricName(group.GroupName, c),
                    c.Value, c.Unit, c.Tags));

                ReportSection("Meters", group.Meters, m => ReportMeter(FormatMetricName(group.GroupName, m),
                    m.Value, m.Unit, m.RateUnit, m.Tags));

                ReportSection("Histograms", group.Histograms, h => ReportHistogram(FormatMetricName(group.GroupName, h),
                    h.Value, h.Unit, h.Tags));

                ReportSection("Timers", group.Timers, t => ReportTimer(FormatMetricName(group.GroupName, t),
                    t.Value, t.Unit, t.RateUnit, t.DurationUnit, t.Tags));

                EndContext(group.GroupName);

            }

        }

        private async Task ReportHealthStatus(IHealthCheckManager healthCheckManager)
        {
            var status = await healthCheckManager.GetStatusAsync();

            if (!status.HasRegisteredChecks)
            {
                return;
            }
            StartMetricGroup("Health Checks");
            ReportHealth(status);
        }

        private void ReportSection<T>(string name, IEnumerable<T> metrics, Action<T> reporter)
        {
            if (_token.IsCancellationRequested)
            {
                return;
            }

            if (!metrics.Any()) return;

            StartMetricGroup(name);
            foreach (var metric in metrics)
            {
                if (_token.IsCancellationRequested)
                {
                    break;
                }

                reporter(metric);
            }
            EndMetricGroup(name);
        }
    }
}