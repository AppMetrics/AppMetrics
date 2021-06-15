// <copyright file="MetricsStatsDStringOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Formatting.StatsD.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatting.StatsD
{
    public class MetricsStatsDStringOutputFormatter : IMetricsChunkedOutputFormatter
    {
        private readonly MetricsStatsDOptions _options;
        private readonly StatsDPointSampler _samplers;
        private readonly MetricSnapshotStatsDStringWriter _nullWriter;
        private readonly MetricSnapshotSerializer _serializer;

        public MetricsStatsDStringOutputFormatter() 
            : this(new MetricsStatsDOptions(), null)
        {
        }

        public MetricsStatsDStringOutputFormatter(MetricFields metricFields) 
            : this(new MetricsStatsDOptions(), metricFields)
        {
        }

        public MetricsStatsDStringOutputFormatter(MetricsStatsDOptions options) 
            : this(options, null)
        {
        }

        public MetricsStatsDStringOutputFormatter(
            MetricsStatsDOptions options,
            MetricFields metricFields)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            MetricFields = metricFields ?? new MetricFields();
            _samplers = new StatsDPointSampler(_options);
            _nullWriter = new MetricSnapshotStatsDStringWriter(null, _samplers, _options);
            _serializer = new MetricSnapshotSerializer();
        }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics.statsd", "v1", "plain");

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; }

        /// <inheritdoc />
        public async Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            await using var writer = new MetricSnapshotStatsDStringWriter(output, _samplers, _options);
            _serializer.Serialize(writer, metricsData, MetricFields);
        }

        public Task<List<string>> WriteAsync(
            MetricsDataValueSource metricsData, 
            int maxChunkSize,
            CancellationToken cancellationToken = default)
        {
            _serializer.Serialize(_nullWriter, metricsData, MetricFields);

            var chunks = new List<string>();
            while (_samplers.Points.TryDequeue(out var point))
            {
                chunks.Add(point.Write(_samplers.Options));
            }

            // Shortcut, no need to calculate batch for empty or single result.
            if (chunks.Count < 2)
            {
                return Task.FromResult(chunks);
            }

            var result = new List<string>();
            var a = chunks[0];
            for (var i = 1; i < chunks.Count; i++)
            {
                var b = chunks[i];
                var joined = $"{a}\n{b}";
                if (joined.Length > maxChunkSize)
                {
                    result.Add(a);
                    a = b;
                }
                else
                {
                    a = joined;
                }
            }
            result.Add(a);

            return Task.FromResult(result);
        }
    }
}
