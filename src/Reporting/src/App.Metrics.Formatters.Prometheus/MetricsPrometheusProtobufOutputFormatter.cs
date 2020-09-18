// <copyright file="MetricsPrometheusProtobufOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.Prometheus.Internal;
using App.Metrics.Formatters.Prometheus.Internal.Extensions;

namespace App.Metrics.Formatters.Prometheus
{
    public class MetricsPrometheusProtobufOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsPrometheusOptions _options;

        public MetricsPrometheusProtobufOutputFormatter()
        {
            _options = new MetricsPrometheusOptions();
        }

        public MetricsPrometheusProtobufOutputFormatter(MetricsPrometheusOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics.prometheus", "v1", "vnd.google.protobuf; proto=io.prometheus.client.MetricFamily; encoding=delimited");

        /// <inheritdoc/>
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

            var bodyData = ProtoFormatter.Format(metricsData.GetPrometheusMetricsSnapshot(_options.MetricNameFormatter,
                _options.LabelNameFormatter));
            return output.WriteAsync(bodyData, 0, bodyData.Length, cancellationToken);
        }
    }
}