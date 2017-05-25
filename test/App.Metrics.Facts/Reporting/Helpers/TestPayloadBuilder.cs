using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestPayloadBuilder : IMetricPayloadBuilder<TestMetricPayload>
    {
        private TestMetricPayload _payload;
        private readonly Func<string, string, string> _metricNameFormatter = (metricContext, metricName) => metricContext.IsMissing()
            ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
            : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();

        public TestPayloadBuilder()
            :this (new MetricValueDataKeys())
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

        public void Clear() { _payload = null; }

        public void Init()
        {
            _payload = new TestMetricPayload();
        }

        public MetricValueDataKeys DataKeys { get; }


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
            var fields = columns.Zip(values, (column, data) => new { column, data })
                                .ToDictionary(pair => pair.column, pair => pair.data);

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