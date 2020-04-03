// <copyright file="MetricsDatadogJsonOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Serialization;

namespace App.Metrics.Formatting.Datadog
{
    public class MetricsDatadogJsonOutputFormatter : IMetricsOutputFormatter
    {
        private readonly TimeSpan _flushInterval;
        private readonly MetricsDatadogOptions _options;

        public MetricsDatadogJsonOutputFormatter(TimeSpan flushInterval)
        {
            _flushInterval = flushInterval;
            _options = new MetricsDatadogOptions();
        }

        public MetricsDatadogJsonOutputFormatter(TimeSpan flushInterval, MetricFields metricFields)
        {
            _flushInterval = flushInterval;
            _options = new MetricsDatadogOptions();
            MetricFields = metricFields;
        }

        public MetricsDatadogJsonOutputFormatter(TimeSpan flushInterval, MetricsDatadogOptions options)
        {
            _flushInterval = flushInterval;
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public MetricsDatadogJsonOutputFormatter(
            TimeSpan flushInterval,
            MetricsDatadogOptions options,
            MetricFields metricFields)
        {
            _flushInterval = flushInterval;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            MetricFields = metricFields;
        }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics.datadog", "v1", "json");

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

            await using var jsonWriter = new MetricSnapshotDatadogJsonWriter(
                output,
                _flushInterval,
                _options.MetricNameFormatter);
                
            serializer.Serialize(jsonWriter, metricsData, MetricFields);
        }
    }
}