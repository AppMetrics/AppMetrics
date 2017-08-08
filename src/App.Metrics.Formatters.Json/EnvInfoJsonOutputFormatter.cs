// <copyright file="EnvInfoJsonOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
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

        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.env", "v1", "json");

        public Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default(CancellationToken))
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

            return Task.CompletedTask;
        }
    }
}
