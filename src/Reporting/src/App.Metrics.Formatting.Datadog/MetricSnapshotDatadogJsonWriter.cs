// <copyright file="MetricSnapshotHostedMetricsJsonWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatting.Datadog.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatting.Datadog
{
    public class MetricSnapshotDatadogJsonWriter : IMetricSnapshotWriter
    {
        private readonly Stream _stream;
        private readonly TimeSpan _flushInterval;
        private readonly IDatadogMetricJsonWriter _metricMetricJsonWriter;
        private readonly DatadogSeries _series;

        public MetricSnapshotDatadogJsonWriter(
            Stream stream,
            TimeSpan flushInterval,
            Func<IDatadogMetricJsonWriter> metricPointTextWriter = null)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _flushInterval = flushInterval;
            _series = new DatadogSeries();

            _metricMetricJsonWriter = metricPointTextWriter != null ? metricPointTextWriter() : DatadogFormatterConstants.GraphiteDefaults.MetricPointTextWriter();
        }

        /// <inheritdoc />
        public void Write(string context, string name, string field, object value, MetricTags tags, DateTime timestamp)
        {
            _series.Add(new DatadogPoint(context, name, new Dictionary<string, object> { { "value", value } }, tags, _metricMetricJsonWriter, _flushInterval, timestamp));
        }

        /// <inheritdoc />
        public void Write(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags, DateTime timestamp)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _series.Add(new DatadogPoint(context, name, fields, tags, _metricMetricJsonWriter, _flushInterval, timestamp));
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _series.WriteAsync(_stream);
#if NETSTANDARD2_1
                _stream?.DisposeAsync();
#else
                _stream?.Dispose();
#endif
            }
        }

        public ValueTask DisposeAsync()
        {
            return DisposeAsync(true);
        }
    }
}
