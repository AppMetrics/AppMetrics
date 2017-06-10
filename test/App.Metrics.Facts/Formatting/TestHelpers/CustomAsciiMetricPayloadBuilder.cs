// <copyright file="CustomAsciiMetricPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Facts.Formatting.TestHelpers
{
    public class CustomAsciiMetricPayloadBuilder : IMetricPayloadBuilder<CustomAsciiMetricPayload>
    {
        private readonly Func<string, string, string> _metricNameFormatter;
        private CustomAsciiMetricPayload _payload;

        public CustomAsciiMetricPayloadBuilder()
        {
            _payload = new CustomAsciiMetricPayload();
            _metricNameFormatter = (metricContext, metricName) => StringExtensions.IsMissing(metricContext)
                ? metricName
                : $"[{metricContext}] {metricName}";
            DataKeys = new MetricValueDataKeys();
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
            _payload = new CustomAsciiMetricPayload();
        }

        /// <inheritdoc />
        public void Pack(string context, string name, object value, MetricTags tags)
        {
            var measurement = _metricNameFormatter(context, name);
            _payload.Add(new CustomAsciiMetricPoint(measurement, new Dictionary<string, object> { { "value", value } }, tags));
        }

        /// <inheritdoc />
        public void Pack(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            var measurement = _metricNameFormatter(context, name);

            _payload.Add(new CustomAsciiMetricPoint(measurement, fields, tags));
        }

        /// <inheritdoc />
        public CustomAsciiMetricPayload Payload()
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