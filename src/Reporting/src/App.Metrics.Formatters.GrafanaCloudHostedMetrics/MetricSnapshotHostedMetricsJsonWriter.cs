// <copyright file="MetricSnapshotHostedMetricsJsonWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public class MetricSnapshotHostedMetricsJsonWriter : IMetricSnapshotWriter
    {
        private readonly Stream _stream;
        private readonly TimeSpan _flushInterval;
        private readonly IHostedMetricsPointTextWriter _metricPointTextWriter;
        private readonly HostedMetricsPoints _points;

        public MetricSnapshotHostedMetricsJsonWriter(
            Stream stream,
            TimeSpan flushInterval,
            Func<IHostedMetricsPointTextWriter> metricPointTextWriter = null)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _flushInterval = flushInterval;
            _points = new HostedMetricsPoints();

            _metricPointTextWriter = metricPointTextWriter != null ? metricPointTextWriter() : HostedMetricsFormatterConstants.GraphiteDefaults.MetricPointTextWriter();
        }

        /// <inheritdoc />
        public void Write(string context, string name, string field, object value, MetricTags tags, DateTime timestamp)
        {
            _points.Add(new HostedMetricsPoint(context, name, new Dictionary<string, object> { { "value", value } }, tags, _metricPointTextWriter, _flushInterval, timestamp));
        }

        /// <inheritdoc />
        public void Write(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags, DateTime timestamp)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _points.Add(new HostedMetricsPoint(context, name, fields, tags, _metricPointTextWriter, _flushInterval, timestamp));
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
                await _points.WriteAsync(_stream);
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
