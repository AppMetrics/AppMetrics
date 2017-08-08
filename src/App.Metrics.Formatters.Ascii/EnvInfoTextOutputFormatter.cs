// <copyright file="EnvInfoTextOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using App.Metrics.Serialization;

namespace App.Metrics.Formatters.Ascii
{
    public class EnvInfoTextOutputFormatter : IEnvOutputFormatter
    {
        private readonly MetricsTextOptions _options;

        public EnvInfoTextOutputFormatter()
        {
            _options = new MetricsTextOptions();
        }

        public EnvInfoTextOutputFormatter(MetricsTextOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.env", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new DefaultEnvInfoSerializer();

            using (var stringWriter = new StreamWriter(output, _options.Encoding))
            {
                using (var textWriter = new EnvInfoTextWriter(stringWriter, _options.Separator, _options.Padding))
                {
                    serializer.Serialize(textWriter, environmentInfo);
                }
            }

            return Task.CompletedTask;
        }
    }
}
