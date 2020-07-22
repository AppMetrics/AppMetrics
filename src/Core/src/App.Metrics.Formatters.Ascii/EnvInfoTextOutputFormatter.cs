// <copyright file="EnvInfoTextOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
#if !NETSTANDARD1_6
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
        public async Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new EnvironmentInfoSerializer();
#if NETSTANDARD2_1
            await using (var stringWriter = new StreamWriter(output, _options.Encoding))
#else
            using (var stringWriter = new StreamWriter(output, _options.Encoding))
#endif
            {
#if NETSTANDARD2_1
                await using (var textWriter = new EnvInfoTextWriter(stringWriter, _options.Separator, _options.Padding))
#else
                using (var textWriter = new EnvInfoTextWriter(stringWriter, _options.Separator, _options.Padding))
#endif
                {
                    await serializer.Serialize(textWriter, environmentInfo);
                }
            }
        }
    }
}
