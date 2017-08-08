// <copyright file="MetricsTextOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Ascii
{
    public class MetricsTextOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsTextOptions _options;

        public MetricsTextOutputFormatter()
        {
            _options = new MetricsTextOptions();
        }

        public MetricsTextOutputFormatter(MetricsTextOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new DefaultMetricSnapshotSerializer();

            using (var streamWriter = new StreamWriter(output, _options.Encoding))
            {
                using (var textWriter = new MetricSnapshotTextWriter(
                    streamWriter,
                    _options.Separator,
                    _options.Padding,
                    _options.MetricNameFormatter,
                    _options.DataKeys))
                {
                    serializer.Serialize(textWriter, metricsData);
                }
            }

            return Task.CompletedTask;
        }
    }
}