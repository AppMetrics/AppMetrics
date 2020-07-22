// <copyright file="MetricSnapshotGraphitePlainTextProtocolWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatters.Graphite.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Graphite
{
    public class MetricSnapshotGraphitePlainTextProtocolWriter : IMetricSnapshotWriter
    {
        private readonly TextWriter _textWriter;
        private readonly IGraphitePointTextWriter _metricNameFormatter;
        private readonly GraphitePoints _points;

        public MetricSnapshotGraphitePlainTextProtocolWriter(
            TextWriter textWriter,
            IGraphitePointTextWriter metricNameFormatter = null)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _points = new GraphitePoints();

            _metricNameFormatter = metricNameFormatter ?? new DefaultGraphitePointTextWriter();
        }

        /// <inheritdoc />
        public void Write(string context, string name, string field, object value, MetricTags tags, DateTime timestamp)
        {
            _points.Add(new GraphitePoint(context, name, new Dictionary<string, object> { { field, value } }, tags, _metricNameFormatter, timestamp));
        }

        /// <inheritdoc />
        public void Write(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags, DateTime timestamp)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _points.Add(new GraphitePoint(context, name, fields, tags, _metricNameFormatter, timestamp));
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
                await _points.WriteAsync(_textWriter);
#if NETSTANDARD2_1
                _textWriter?.DisposeAsync();
#else
                _textWriter?.Dispose();
#endif
            }
        }

        public ValueTask DisposeAsync()
        {
            return DisposeAsync(true);
        }
    }
}
