// <copyright file="MetricSnapshotStatsDStringWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using App.Metrics.Formatting.StatsD.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatting.StatsD
{
    internal class MetricSnapshotStatsDStringWriter : IMetricSnapshotWriter
    {
        private readonly IStatsDMetricStringSerializer _metricMetricStringSerializer;
        private readonly MetricsStatsDOptions _options;
        private readonly Stream _stream;

        public MetricSnapshotStatsDStringWriter(Stream stream, StatsDPointSampler sampler, MetricsStatsDOptions options)
        {
            _stream = stream;
            Sampler = sampler;
            _options = options;
            _metricMetricStringSerializer = options.MetricNameFormatter ?? StatsDFormatterConstants.Defaults.MetricPointTextWriter;
        }

        public StatsDPointSampler Sampler { get; }

        public ValueTask DisposeAsync() { return DisposeAsync(true); }

        /// <inheritdoc />
        public void Write(string context, string name, string field, object value, MetricTags tags, DateTime timestamp)
        {
            Sampler.Add(context, name, field, value, tags, _metricMetricStringSerializer, timestamp);
        }

        /// <inheritdoc />
        public void Write(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags, DateTime timestamp)
        {
            Sampler.Add(context, name, columns, values, tags, _metricMetricStringSerializer, timestamp);
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
                if (_stream != null)
                {
                    await Sampler.WriteAsync(_stream);
#if NETSTANDARD2_1
                    await _stream.DisposeAsync();
#else
                    _stream.Dispose();
#endif
                }
            }
        }
    }
}