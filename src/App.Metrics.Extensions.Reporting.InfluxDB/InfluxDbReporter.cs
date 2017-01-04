// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Extensions.Reporting.InfluxDB.Extensions;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporter : IMetricReporter
    {
        private readonly LineProtocolClient _influxDbClient;
        private readonly ILogger<InfluxDbReporter> _logger;
        private bool _disposed;
        private LineProtocolPayload _payload;

        public InfluxDbReporter(InfluxDBReporterSettings settings, ILoggerFactory loggerFactory)
            : this(typeof(InfluxDbReporter).Name, settings, loggerFactory)
        {
        }

        public InfluxDbReporter(string name, InfluxDBReporterSettings settings, ILoggerFactory loggerFactory)
        {
            ReportInterval = settings.ReportInterval;
            Name = name;

            _logger = loggerFactory.CreateLogger<InfluxDbReporter>();
            _influxDbClient = new LineProtocolClient(loggerFactory, settings.InfluxDbSettings, settings.HttpPolicy);
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
                    _payload = null;
                }
            }

            _logger.LogDebug($"{Name} Disposed");

            _disposed = true;
        }

        public async Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            _logger.LogDebug($"Ending {Name} Run");

            var result = await _influxDbClient.WriteAsync(_payload);

            return result.Success;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
        }

        public void ReportHealth(GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            _logger.LogDebug($"Packing Health Checks for {Name}");

            var unhealthy = unhealthyChecks as HealthCheck.Result[] ?? unhealthyChecks.ToArray();
            var degraded = degradedChecks as HealthCheck.Result[] ?? degradedChecks.ToArray();

            var isUnhealthy = unhealthy.Any();
            var isDegraded = degraded.Any();
            var healthy = !isUnhealthy && !isDegraded;

            var healthStatusValue = 2;

            if (isUnhealthy)
            {
                healthStatusValue = 3;
            }
            else if (healthy)
            {
                healthStatusValue = 1;
            }

            Pack("health", healthStatusValue, new MetricTags(globalTags));

            var checks = unhealthy.Concat(degraded).Concat(healthyChecks);

            foreach (var healthCheck in checks)
            {
                var tags = new MetricTags(globalTags).With("health_check", healthCheck.Name);

                if (healthCheck.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    Pack("health_checks__unhealhty", healthCheck.Check.Message, tags);
                }
                else if (healthCheck.Check.Status == HealthCheckStatus.Healthy)
                {
                    Pack("health_checks__healthy", healthCheck.Check.Message, tags);
                }
                else if (healthCheck.Check.Status == HealthCheckStatus.Degraded)
                {
                    Pack("health_checks__degraded", healthCheck.Check.Message, tags);
                }
            }

            _logger.LogDebug($"Packed Health Checks for {Name}");
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            _logger.LogDebug($"Packing Metric {typeof(T)} for {Name}");

            var contextFormatted = context.Replace(" ", "_").ToLowerInvariant();

            if (typeof(T) == typeof(double))
            {
                ReportGauge(contextFormatted, valueSource as MetricValueSource<double>);
                return;
            }

            if (typeof(T) == typeof(CounterValue))
            {
                ReportCounter(contextFormatted, valueSource as MetricValueSource<CounterValue>);
                return;
            }

            if (typeof(T) == typeof(MeterValue))
            {
                ReportMeter(contextFormatted, valueSource as MetricValueSource<MeterValue>);
                return;
            }

            if (typeof(T) == typeof(TimerValue))
            {
                ReportTimer(contextFormatted, valueSource as MetricValueSource<TimerValue>);
                return;
            }

            if (typeof(T) == typeof(HistogramValue))
            {
                ReportHistogram(contextFormatted, valueSource as MetricValueSource<HistogramValue>);
                return;
            }

            if (typeof(T) == typeof(ApdexValue))
            {
                ReportApdex(contextFormatted, valueSource as MetricValueSource<ApdexValue>);
                return;
            }

            _logger.LogDebug($"Finished Packing Metric {typeof(T)} for {Name}");
        }

        public void StartReportRun(IMetrics metrics)
        {
            _logger.LogDebug($"Starting {Name} Report Run");

            _payload = new LineProtocolPayload();
        }

        private void Pack(string name, object value, MetricTags tags)
        {
            _payload.Add(new LineProtocolPoint(name, new Dictionary<string, object> { { "value", value } }, tags));
        }

        private void Pack(string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _payload.Add(new LineProtocolPoint(name, fields, tags));
        }

        private void ReportApdex(string name, MetricValueSource<ApdexValue> valueSource)
        {
            var apdexValueSource = valueSource as ApdexValueSource;

            if (apdexValueSource == null)
            {
                return;
            }

            var data = new Dictionary<string, object>();

            valueSource.Value.AddApdexValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"{name}__{valueSource.FormattedMetricName()}", keys, values, valueSource.Tags);
        }

        private void ReportCounter(string name, MetricValueSource<CounterValue> valueSource)
        {
            var counterValueSource = valueSource as CounterValueSource;

            if (counterValueSource == null)
            {
                return;
            }

            if (counterValueSource.Value.Items.Any() && counterValueSource.ReportSetItems)
            {
                foreach (var item in counterValueSource.Value.Items.Distinct())
                {
                    var data = new Dictionary<string, object> { { "total", item.Count } };

                    if (counterValueSource.ReportItemPercentages)
                    {
                        data.Add("percent", item.Percent);
                    }

                    var keys = data.Keys.ToList();
                    var values = keys.Select(k => data[k]);

                    Pack($"{name}__{counterValueSource.FormattedMetricName()}__items", keys, values, item.Tags);
                }
            }

            var count = counterValueSource.ValueProvider.GetValue(resetMetric: counterValueSource.ResetOnReporting).Count;

            Pack($"{name}__{counterValueSource.FormattedMetricName()}", count, valueSource.Tags);
        }

        private void ReportGauge(string name, MetricValueSource<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                Pack($"{name}__{valueSource.FormattedMetricName()}", valueSource.Value, valueSource.Tags);
            }
        }

        private void ReportHistogram(string name, MetricValueSource<HistogramValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"{name}__{valueSource.FormattedMetricName()}", keys, values, valueSource.Tags);
        }

        private void ReportMeter(string name, MetricValueSource<MeterValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            if (valueSource.Value.Items.Any())
            {
                foreach (var item in valueSource.Value.Items.Distinct())
                {
                    var itemData = new Dictionary<string, object>();

                    item.Value.AddMeterValues(itemData);
                    itemData.Add("percent", item.Percent);

                    var itemKeys = itemData.Keys.ToList();
                    var itemValues = itemKeys.Select(k => itemData[k]).ToList();
                    Pack($"{name}__{valueSource.FormattedMetricName()}__items", itemKeys, itemValues, item.Tags);
                }
            }

            valueSource.Value.AddMeterValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"{name}__{valueSource.FormattedMetricName()}", keys, values, valueSource.Tags);
        }

        private void ReportTimer(string name, MetricValueSource<TimerValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.Rate.AddMeterValues(data);
            valueSource.Value.Histogram.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"{name}__{valueSource.FormattedMetricName()}", keys, values, valueSource.Tags);
        }
    }
}