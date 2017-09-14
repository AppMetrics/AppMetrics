// <copyright file="EnvInfoJsonOutputFormatter.cs" company="Allan Hardy">
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
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class EnvInfoJsonOutputFormatter : IEnvOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public EnvInfoJsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        public EnvInfoJsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.env", "v1", "json");

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

            var serilizer = JsonSerializer.Create(_serializerSettings);

            using (var sw = new StreamWriter(output))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    serilizer.Serialize(jw, environmentInfo);
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
