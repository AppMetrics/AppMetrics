// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Extensions.Reporting.InfluxDB.Extensions;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class InfluxDbReporter : IMetricReporter
    {
        private readonly LineProtocolClient _influxDbClient;
        private LineProtocolPayload _payload;
        private bool _disposed;

        public InfluxDbReporter(Uri serverBaseAddress, string username, 
            string password, string database, string breakerRate, TimeSpan interval,
            string retentionPolicy, string consistency)
            : this("InfluxDB Reporter", serverBaseAddress, username, password, 
                   database, breakerRate, interval,
                  retentionPolicy, consistency)
        {
        }

        public InfluxDbReporter(string name, Uri serverBaseAddress,
            string username, string password, string database, string breakerRate, TimeSpan interval,
            string retentionPolicy, string consistency)
        {
            ReportInterval = interval;
            Name = name;

            _influxDbClient = new LineProtocolClient(serverBaseAddress, database, username, password, retentionPolicy, consistency, breakerRate);
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

            _disposed = true;
        }

        public void EndMetricTypeReport(Type metricType)
        {
        }

        public void EndReport(IMetrics metrics)
        {
            _influxDbClient.WriteAsync(_payload).GetAwaiter().GetResult();
            _payload = null;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
        }

        public void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
        }

        public void ReportMetric<T>(string name, MetricValueSource<T> valueSource)
        {
            if (typeof(T) == typeof(double))
            {
                ReportGauge(name, valueSource as MetricValueSource<double>);
                return;
            }

            if (typeof(T) == typeof(CounterValue))
            {
                ReportCounter(name, valueSource as MetricValueSource<CounterValue>);
                return;
            }

            if (typeof(T) == typeof(MeterValue))
            {
                ReportMeter(name, valueSource as MetricValueSource<MeterValue>);
                return;
            }

            if (typeof(T) == typeof(TimerValue))
            {
                ReportTimer(name, valueSource as MetricValueSource<TimerValue>);
                return;
            }

            if (typeof(T) == typeof(HistogramValue))
            {
                ReportHistogram(name, valueSource as MetricValueSource<HistogramValue>);
                return;
            }
        }

        public void StartMetricTypeReport(Type metricType)
        {
        }

        public void StartReport(IMetrics metrics)
        {
            _payload = new LineProtocolPayload();
        }

        private void Pack(string name, object value, MetricTags tags)
        {
            _payload.Add(new LineProtocolPoint(name, new Dictionary<string, object> { {"value", value} }, tags.ToDictionary()));
        }

        private void Pack(string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _payload.Add(new LineProtocolPoint(name, fields, tags.ToDictionary()));
        }

        private void ReportCounter(string name, MetricValueSource<CounterValue> valueSource)
        {
            Pack($"[{name}] {valueSource.Name}", valueSource.Value.Count, valueSource.Tags);

            if (!valueSource.Value.Items.Any()) return;

            foreach (var item in valueSource.Value.Items.Distinct())
            {
                var data = new Dictionary<string, object>
                {
                    { "total", item.Count },
                    { "percent", item.Percent }
                };

                var keys = data.Keys.ToList();
                var values = keys.Select(k => data[k]);
                var itemTags = new MetricTags(valueSource.Tags.ToDictionary()).With("item", item.Item);

                Pack($"[{name}] {valueSource.Name} Items", keys, values, itemTags);
            }
        }

        private void ReportGauge(string name, MetricValueSource<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                Pack($"[{name}] {valueSource.Name}", valueSource.Value, valueSource.Tags);
            }
        }

        private void ReportHistogram(string name, MetricValueSource<HistogramValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"[{name}] {valueSource.Name}", keys, values, valueSource.Tags);
        }

        private void ReportMeter(string name, MetricValueSource<MeterValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.AddMeterValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"[{name}] {valueSource.Name}", keys, values, valueSource.Tags);

            foreach (var item in valueSource.Value.Items.Distinct())
            {
                var itemData = new Dictionary<string, object>();

                item.Value.AddMeterValues(itemData);
                itemData.Add("percent", item.Percent);

                var itemKeys = itemData.Keys.ToList();
                var itemValues = itemKeys.Select(k => itemData[k]);
                var itemTags = new MetricTags(valueSource.Tags.ToDictionary()).With("item", item.Item);

                Pack($"[{name}] {valueSource.Name} Items", itemKeys, itemValues, itemTags);
            }
        }

        private void ReportTimer(string name, MetricValueSource<TimerValue> valueSource)
        {
            var data = new Dictionary<string, object>();

            valueSource.Value.Rate.AddMeterValues(data);
            valueSource.Value.Histogram.AddHistogramValues(data);

            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);

            Pack($"[{name}] {valueSource.Name}", keys, values, valueSource.Tags);
        }
    }
}