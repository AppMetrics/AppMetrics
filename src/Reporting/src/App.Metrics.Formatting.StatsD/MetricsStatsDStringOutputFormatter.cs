// <copyright file="MetricsStatsDStringOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Formatting.StatsD.Internal;
using App.Metrics.Serialization;

namespace App.Metrics.Formatting.StatsD
{
    public class MetricsStatsDStringOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsStatsDOptions _options;
        private readonly StatsDPointSampler _samplers;

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

            var serializer = new MetricSnapshotSerializer();

            await using var writer = new MetricSnapshotStatsDStringWriter(output, _samplers, _options);
            serializer.Serialize(writer, metricsData, MetricFields);
        }
    }
}
