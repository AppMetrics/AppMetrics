// <copyright file="AsciiOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Ascii
{
    public class AsciiOutputFormatter : IMetricsOutputFormatter
    {
        private readonly MetricsAsciiOptions _options;

        public AsciiOutputFormatter(MetricsAsciiOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.metrics", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var payloadBuilder = new AsciiMetricPayloadBuilder();
            var formatter = new MetricDataValueSourceFormatter();

            formatter.Build(metricsData, payloadBuilder);

            var bytes = encoding.GetBytes(payloadBuilder.PayloadFormatted());

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
