// <copyright file="MetricsTextOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
        }

        public MetricsTextOutputFormatter(MetricsTextOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics", "v1", "plain");

        /// <inheritdoc />
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

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsTaskHelper.CompletedTask();
#endif
        }
    }
}