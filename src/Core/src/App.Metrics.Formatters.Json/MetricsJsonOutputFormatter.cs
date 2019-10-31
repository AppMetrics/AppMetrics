// <copyright file="MetricsJsonOutputFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Json
{
    public class MetricsJsonOutputFormatter : IMetricsOutputFormatter
    {
        private readonly JsonSerializerOptions _serializerSettings;

        public MetricsJsonOutputFormatter(JsonSerializerOptions serializerSettings)
        {
            _serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
        }

        public MetricsJsonOutputFormatter() { _serializerSettings = DefaultJsonSerializerSettings.CreateSerializerOptions(); }

        /// <inheritdoc />
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("application", "vnd.appmetrics.metrics", "v1", "json");

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; }

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

            // TODO: #251 should apply metric field names
            return JsonSerializer.SerializeAsync(output, metricData, _serializerSettings, cancellationToken: cancellationToken);
        }
    }
}