// <copyright file="MetricsInfluxDBLineProtocolOutputFormatter.cs" company="App Metrics Contributors">
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

namespace App.Metrics.Formatters.InfluxDB
{
    public class MetricsInfluxDbLineProtocolOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsInfluxDbLineProtocolOptions _options;

        public MetricsInfluxDbLineProtocolOutputFormatter()
        {
            _options = new MetricsInfluxDbLineProtocolOptions();
        }

        public MetricsInfluxDbLineProtocolOutputFormatter(MetricFields metricFields)
        {
            _options = new MetricsInfluxDbLineProtocolOptions();
            MetricFields = metricFields;
        }

        public MetricsInfluxDbLineProtocolOutputFormatter(MetricsInfluxDbLineProtocolOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public MetricsInfluxDbLineProtocolOutputFormatter(MetricsInfluxDbLineProtocolOptions options, MetricFields metricFields)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            MetricFields = metricFields;
        }

        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.influxdb", "v1", "plain");

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; }

        /// <inheritdoc/>
        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new MetricSnapshotSerializer();

            using (var streamWriter = new StreamWriter(output))
            {
                using (var textWriter = new MetricSnapshotInfluxDbLineProtocolWriter(
                    streamWriter,
                    _options.MetricNameFormatter))
                {
                    serializer.Serialize(textWriter, metricsData, MetricFields);
                }
            }

#if !NETSTANDARD1_6
            return AppMetricsTaskHelper.CompletedTask();
#else
            return Task.CompletedTask;
#endif
        }
    }
}