// <copyright file="MetricsJsonOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6
using App.Metrics.Internal;
#endif
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class MetricsJsonOutputFormatter : IMetricsOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public MetricsJsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public MetricsJsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics", "v1", "json");

        /// <inheritdoc />
        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricData,
            CancellationToken cancellationToken = default)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var serilizer = JsonSerializer.Create(_serializerSettings);

            using (var streamWriter = new StreamWriter(output))
            {
                using (var textWriter = new JsonTextWriter(streamWriter))
                {
                    serilizer.Serialize(textWriter, metricData);
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