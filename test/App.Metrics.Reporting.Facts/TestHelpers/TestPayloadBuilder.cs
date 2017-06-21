// <copyright file="TestPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public class TestPayloadBuilder : IMetricPayloadBuilder<TestMetricPayload>
    {
        private readonly Func<string, string, string> _metricNameFormatter = (metricContext, metricName) => metricContext.IsMissing()
            ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
            : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();

        private TestMetricPayload _payload;

        public TestPayloadBuilder()
            : this(new MetricValueDataKeys())
        {
        }

        public TestPayloadBuilder(MetricValueDataKeys dataKeys)
        {
            _payload = new TestMetricPayload();
            DataKeys = dataKeys ?? new MetricValueDataKeys();
        }

        public TestPayloadBuilder(Func<string, string, string> metricNameFormatter, MetricValueDataKeys dataKeys)
        {
            _payload = new TestMetricPayload();
            DataKeys = dataKeys ?? new MetricValueDataKeys();
            _metricNameFormatter = metricNameFormatter;
        }

        public MetricValueDataKeys DataKeys { get; }

        public void Clear() { _payload = null; }

        public void Init() { _payload = new TestMetricPayload(); }

        public void Pack(string context, string name, object value, MetricTags tags)
        {
            var measurement = _metricNameFormatter(context, name);

            _payload.Add(new TestMetricPoint(measurement, new Dictionary<string, object> { { "value", value } }, tags));
        }

        public void Pack(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            var measurement = _metricNameFormatter(context, name);

            _payload.Add(new TestMetricPoint(measurement, fields, tags));
        }

        public TestMetricPayload Payload() { return _payload; }

        public string PayloadFormatted()
        {
            var result = new StringBuilder();
            _payload.Format(result);
            return result.ToString();
        }
    }
}