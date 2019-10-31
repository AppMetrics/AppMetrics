// <copyright file="EnvInfoJsonOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;

namespace App.Metrics.Formatters.Json
{
    public class EnvInfoJsonOutputFormatter : IEnvOutputFormatter
    {
        private readonly JsonSerializerOptions _serializerSettings;

        public EnvInfoJsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerOptions(); }

        public EnvInfoJsonOutputFormatter(JsonSerializerOptions serializerSettings)
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

            return JsonSerializer.SerializeAsync(output, environmentInfo, _serializerSettings, cancellationToken: cancellationToken);
        }
    }
}
