// <copyright file="MetricsHostedMetricsJsonOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Serialization;
using App.Metrics.Internal;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public class MetricsHostedMetricsJsonOutputFormatter : IMetricsOutputFormatter
    {
        private readonly TimeSpan _flushInterval;
        private readonly MetricsHostedMetricsOptions _options;

        public MetricsHostedMetricsJsonOutputFormatter(TimeSpan flushInterval)
        {
            _flushInterval = flushInterval;
            _options = new MetricsHostedMetricsOptions();
        }

        public MetricsHostedMetricsJsonOutputFormatter(TimeSpan flushInterval, MetricFields metricFields)
        {
            _flushInterval = flushInterval;
            _options = new MetricsHostedMetricsOptions();
            MetricFields = metricFields;
        }

        public MetricsHostedMetricsJsonOutputFormatter(TimeSpan flushInterval, MetricsHostedMetricsOptions options)
        {
            _flushInterval = flushInterval;
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public MetricsHostedMetricsJsonOutputFormatter(
            TimeSpan flushInterval,
            MetricsHostedMetricsOptions options,
            MetricFields metricFields)
        {
            _flushInterval = flushInterval;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            MetricFields = metricFields;
        }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.hostedmetrics", "v1", "plain");

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; } = new MetricFields();

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

            await using var jsonWriter = new MetricSnapshotHostedMetricsJsonWriter(
                output,
                _flushInterval,
                _options.MetricNameFormatter);
                
            serializer.Serialize(jsonWriter, metricsData, MetricFields);
        }
    }
}