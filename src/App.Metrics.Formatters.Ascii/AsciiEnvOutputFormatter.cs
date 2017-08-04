// <copyright file="AsciiEnvOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Formatters.Ascii
{
    public class AsciiEnvOutputFormatter : IEnvOutputFormatter
    {
        private readonly MetricsAsciiOptions _options;

        public AsciiEnvOutputFormatter()
        {
            _options = new MetricsAsciiOptions();
        }

        public AsciiEnvOutputFormatter(MetricsAsciiOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.env", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
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

            using (var result = new StringWriter())
            {
                var formatter = new AsciiEnvironmentInfoFormatter(environmentInfo);
                formatter.Format(result);

                var bytes = encoding.GetBytes(result.ToString());

                return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            }
        }
    }
}
