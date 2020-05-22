// <copyright file="MetricSnapshotTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatters.Ascii.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Ascii
{
    public class MetricSnapshotTextWriter : IMetricSnapshotWriter
    {
        private readonly string _separator;
        private readonly int _padding;
        private readonly TextWriter _textWriter;
        private readonly Func<string, string, string> _metricNameFormatter;
        private readonly MetricsTextPoints _textPoints;

        public MetricSnapshotTextWriter(
            TextWriter textWriter,
            string separator = MetricsTextFormatterConstants.OutputFormatting.Separator,
            int padding = MetricsTextFormatterConstants.OutputFormatting.Padding,
            Func<string, string, string> metricNameFormatter = null)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _separator = separator;
            _padding = padding;
            _textPoints = new MetricsTextPoints();
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
        }

        /// <inheritdoc />
        public void Write(
            string context,
            string name,
            string field,
            object value,
            MetricTags tags,
            DateTime timestamp)
        {
            if (value == null)
            {
                return;
            }

            var measurement = _metricNameFormatter(context, name);

            _textPoints.Add(new MetricsTextPoint(measurement, new Dictionary<string, object> { { field, value } }, tags, timestamp));
        }

        /// <inheritdoc />
        public void Write(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags,
            DateTime timestamp)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            if (!fields.Any())
            {
                return;
            }

            var measurement = _metricNameFormatter(context, name);

            _textPoints.Add(new MetricsTextPoint(measurement, fields, tags, timestamp));
        }

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            return DisposeAsync(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _textPoints.WriteAsync(_textWriter, _separator, _padding);
                _textWriter?.Dispose();
            }
        }
    }
}