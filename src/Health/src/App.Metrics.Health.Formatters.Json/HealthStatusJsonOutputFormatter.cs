// <copyright file="HealthStatusJsonOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
#if !NETSTANDARD1_6
using App.Metrics.Health.Internal;
#endif

namespace App.Metrics.Health.Formatters.Json
{
    public class HealthStatusJsonOutputFormatter : IHealthOutputFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public HealthStatusJsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerSettings(); }

        public HealthStatusJsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public HealthMediaTypeValue MediaType => new HealthMediaTypeValue("application", "vnd.appmetrics.health", "v1", "json");

        public Task WriteAsync(
            Stream output,
            HealthStatus healthStatus,
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
                    serilizer.Serialize(textWriter, healthStatus);
                }
            }

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }
    }
}