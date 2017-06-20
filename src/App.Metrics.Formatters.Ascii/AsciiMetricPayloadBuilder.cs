// <copyright file="AsciiMetricPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Reporting;

namespace App.Metrics.Formatters.Ascii
{
    public class AsciiMetricPayloadBuilder : IMetricPayloadBuilder<AsciiMetricPayload>
    {
        private readonly Func<string, string, string> _metricNameFormatter;
        private AsciiMetricPayload _payload;

        public AsciiMetricPayloadBuilder(Func<string, string, string> metricNameFormatter = null, MetricValueDataKeys dataKeys = null)
        {
            _payload = new AsciiMetricPayload();
            if (metricNameFormatter == null)
            {
                _metricNameFormatter = (metricContext, metricName) => string.IsNullOrWhiteSpace(metricContext)
                    ? metricName
                    : $"[{metricContext}] {metricName}";
            }
            else
            {
                _metricNameFormatter = metricNameFormatter;
            }

            DataKeys = dataKeys ?? new MetricValueDataKeys();
        }

        public MetricValueDataKeys DataKeys { get; }

        /// <inheritdoc />
        public void Clear()
        {
            _payload = null;
        }

        /// <inheritdoc />
        public void Init()
        {
            _payload = new AsciiMetricPayload();
        }

        /// <inheritdoc />
        public void Pack(string context, string name, object value, MetricTags tags)
        {
            var measurement = _metricNameFormatter(context, name);
            _payload.Add(new AsciiMetricPoint(measurement, new Dictionary<string, object> { { "value", value } }, tags));
        }

        /// <inheritdoc />
        public void Pack(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            var measurement = _metricNameFormatter(context, name);

            _payload.Add(new AsciiMetricPoint(measurement, fields, tags));
        }

        /// <inheritdoc />
        public AsciiMetricPayload Payload()
        {
            return _payload;
        }

        /// <inheritdoc />
        public string PayloadFormatted()
        {
            var result = new StringWriter();
            _payload.Format(result);
            return result.ToString();
        }
    }
}