// <copyright file="MetricSnapshotTextWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Func<string, string, string> metricNameFormatter = null,
            GeneratedMetricNameMapping dataKeys = null)
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

            MetricNameMapping = dataKeys ?? new GeneratedMetricNameMapping();
        }

        /// <inheritdoc />
        public GeneratedMetricNameMapping MetricNameMapping { get; }

        /// <inheritdoc />
        public void Write(
            string context,
            string name,
            object value,
            MetricTags tags,
            DateTime timestamp)
        {
            var measurement = _metricNameFormatter(context, name);

            _textPoints.Add(new MetricsTextPoint(measurement, new Dictionary<string, object> { { "value", value } }, tags, timestamp));
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

            var measurement = _metricNameFormatter(context, name);

            _textPoints.Add(new MetricsTextPoint(measurement, fields, tags, timestamp));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _textPoints.Write(_textWriter, _separator, _padding);
                _textWriter?.Dispose();
            }
        }
    }
}