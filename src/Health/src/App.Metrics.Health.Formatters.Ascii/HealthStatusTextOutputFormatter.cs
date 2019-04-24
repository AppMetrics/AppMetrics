// <copyright file="HealthStatusTextOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Serialization;
#if !NETSTANDARD1_6
using App.Metrics.Health.Internal;
#endif

namespace App.Metrics.Health.Formatters.Ascii
{
    public class HealthStatusTextOutputFormatter : IHealthOutputFormatter
    {
        private readonly HealthTextOptions _options;

        public HealthStatusTextOutputFormatter()
        {
            _options = new HealthTextOptions();
        }

        public HealthStatusTextOutputFormatter(HealthTextOptions options) { _options = options ?? throw new ArgumentNullException(nameof(options)); }

        public HealthMediaTypeValue MediaType => new HealthMediaTypeValue("text", "vnd.appmetrics.health", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            HealthStatus healthStatus,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serializer = new HealthStatusSerializer();

            using (var stringWriter = new StreamWriter(output, _options.Encoding))
            {
                using (var textWriter = new HealthStatusTextWriter(stringWriter, _options.Separator, _options.Padding))
                {
                    serializer.Serialize(textWriter, healthStatus);
                }
            }

#if !NETSTANDARD1_6
            return AppMetricsHealthTaskHelper.CompletedTask();
#else
            return Task.CompletedTask;
#endif
        }
    }
}