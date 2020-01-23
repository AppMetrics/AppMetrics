// <copyright file="MetricsTextOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Internal;
#endif
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Ascii
{
    public class MetricsTextOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsTextOptions _options;

        public MetricsTextOutputFormatter()
        {
            _options = new MetricsTextOptions();
            MetricFields = new MetricFields();
        }

        public MetricsTextOutputFormatter(MetricFields metricFields)
        {
            _options = new MetricsTextOptions();
            MetricFields = metricFields ?? new MetricFields();
        }

        public MetricsTextOutputFormatter(MetricsTextOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public MetricsTextOutputFormatter(MetricsTextOptions options, MetricFields metricFields)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            MetricFields = metricFields ?? new MetricFields();
        }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics", "v1", "plain");

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

            using var streamWriter = new StreamWriter(output, _options.Encoding, bufferSize: 1024, leaveOpen: true);
            
            await using var textWriter = new MetricSnapshotTextWriter(
                streamWriter,
                _options.Separator,
                _options.Padding,
                _options.MetricNameFormatter);
                
            serializer.Serialize(textWriter, metricsData, MetricFields);
        }
    }
}