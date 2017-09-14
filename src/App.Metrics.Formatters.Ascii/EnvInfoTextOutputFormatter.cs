// <copyright file="EnvInfoTextOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
#if !NETSTANDARD1_6
using App.Metrics.Internal;
#endif
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

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.env", "v1", "plain");

        /// <inheritdoc />
        public Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new EnvironmentInfoSerializer();

            using (var stringWriter = new StreamWriter(output, _options.Encoding))
            {
                using (var textWriter = new EnvInfoTextWriter(stringWriter, _options.Separator, _options.Padding))
                {
                    serializer.Serialize(textWriter, environmentInfo);
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
