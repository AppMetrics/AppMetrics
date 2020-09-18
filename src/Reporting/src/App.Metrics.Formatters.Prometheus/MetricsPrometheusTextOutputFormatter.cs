// <copyright file="MetricsPrometheusTextOutputFormatter.cs" company="App Metrics Contributors">
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
    public class MetricsPrometheusTextOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsPrometheusOptions _options;

        public MetricsPrometheusTextOutputFormatter()
            : this(new MetricsPrometheusOptions())
        {
        }

        public MetricsPrometheusTextOutputFormatter(MetricsPrometheusOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics.prometheus", "v1", "plain");

        /// <inheritdoc/>
        public MetricFields MetricFields { get; set; }

        /// <inheritdoc/>
        public async Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var prometheusMetricsSnapshot = metricsData.GetPrometheusMetricsSnapshot(_options.MetricNameFormatter,
                _options.LabelNameFormatter);

            await AsciiFormatter.Write(output, prometheusMetricsSnapshot, _options.NewLineFormat);
        }
    }
}