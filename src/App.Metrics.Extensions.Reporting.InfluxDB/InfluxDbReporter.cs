// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Extensions.Reporting.InfluxDB.Extensions;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporter : IMetricReporter
    {
        private readonly ILineProtocolClient _lineProtocolClient;
        private readonly ILogger<InfluxDbReporter> _logger;
        private readonly Func<string, string, string> _metricNameFormatter;
        private readonly ILineProtocolPayloadBuilder _payloadBuilder;
        private bool _disposed;

        public InfluxDbReporter(
            ILineProtocolClient lineProtocolClient,
            ILineProtocolPayloadBuilder payloadBuilder,
            TimeSpan reportInterval,
            ILoggerFactory loggerFactory,
            Func<string, string, string> metricNameFormatter)
            : this(
                lineProtocolClient,
                payloadBuilder,
                reportInterval,
                typeof(InfluxDbReporter).Name,
                loggerFactory,
                metricNameFormatter) { }

        public InfluxDbReporter(
            ILineProtocolClient lineProtocolClient,
            ILineProtocolPayloadBuilder payloadBuilder,
            TimeSpan reportInterval,
            string name,
            ILoggerFactory loggerFactory,
            Func<string, string, string> metricNameFormatter)
        {
            ReportInterval = reportInterval;
            Name = name;

            _payloadBuilder = payloadBuilder;
            _metricNameFormatter = metricNameFormatter;
            _logger = loggerFactory.CreateLogger<InfluxDbReporter>();
            _lineProtocolClient = lineProtocolClient;
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
                    _payloadBuilder.Clear();
                }
            }

            _logger.LogDebug($"{Name} Disposed");

            _disposed = true;
        }

        public async Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            _logger.LogDebug($"Ending {Name} Run");

            var result = await _lineProtocolClient.WriteAsync(_payloadBuilder.Payload());

            _payloadBuilder.Clear();

            return result.Success;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo) { }

        public void ReportHealth(
            GlobalMetricTags globalTags,
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

            _payloadBuilder.Pack("health", healthStatusValue, new MetricTags(globalTags));

            var checks = unhealthy.Concat(degraded).Concat(healthyChecks);

            foreach (var healthCheck in checks)
            {
                var tags = new MetricTags(globalTags).With("health_check", healthCheck.Name);

                if (healthCheck.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    _payloadBuilder.Pack("health_checks__unhealhty", healthCheck.Check.Message, tags);
                }
                else if (healthCheck.Check.Status == HealthCheckStatus.Healthy)
                {
                    _payloadBuilder.Pack("health_checks__healthy", healthCheck.Check.Message, tags);
                }
                else if (healthCheck.Check.Status == HealthCheckStatus.Degraded)
                {
                    _payloadBuilder.Pack("health_checks__degraded", healthCheck.Check.Message, tags);
                }
            }

            _logger.LogDebug($"Packed Health Checks for {Name}");
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
            _logger.LogDebug($"Packing Metric {typeof(T)} for {Name}");

            if (typeof(T) == typeof(double))
            {
                ReportGauge(context, valueSource as MetricValueSource<double>);
                return;
            }

            if (typeof(T) == typeof(CounterValue))
            {
                ReportCounter(context, valueSource as MetricValueSource<CounterValue>);
                return;
            }

            if (typeof(T) == typeof(MeterValue))
            {
                ReportMeter(context, valueSource as MetricValueSource<MeterValue>);
                return;
            }

            if (typeof(T) == typeof(TimerValue))
            {
                ReportTimer(context, valueSource as MetricValueSource<TimerValue>);
                return;
            }

            if (typeof(T) == typeof(HistogramValue))
            {
                ReportHistogram(context, valueSource as MetricValueSource<HistogramValue>);
                return;
            }

            if (typeof(T) == typeof(ApdexValue))
            {
                ReportApdex(context, valueSource as MetricValueSource<ApdexValue>);
                return;
            }

            _logger.LogDebug($"Finished Packing Metric {typeof(T)} for {Name}");
        }

        public void StartReportRun(IMetrics metrics)
        {
            _logger.LogDebug($"Starting {Name} Report Run");

            _payloadBuilder.Init();
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

            _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), keys, values, valueSource.Tags);
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

                    _payloadBuilder.Pack(
                        _metricNameFormatter(name, valueSource.Name + "  items"),
                        keys,
                        values,
                        item.Tags);
                }
            }

            var count = counterValueSource.ValueProvider.GetValue(resetMetric: counterValueSource.ResetOnReporting).Count;

            _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), count, valueSource.Tags);
        }

        private void ReportGauge(string name, MetricValueSource<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), valueSource.Value, valueSource.Tags);
            }
        }

        private void ReportHistogram(string name, MetricValueSource<HistogramValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), keys, values, valueSource.Tags);
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
                    _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name + "  items"), itemKeys, itemValues, item.Tags);
                }
            }

            valueSource.Value.AddMeterValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), keys, values, valueSource.Tags);
        }

        private void ReportTimer(string name, MetricValueSource<TimerValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.Rate.AddMeterValues(data);
            valueSource.Value.Histogram.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            _payloadBuilder.Pack(_metricNameFormatter(name, valueSource.Name), keys, values, valueSource.Tags);
        }
    }
}