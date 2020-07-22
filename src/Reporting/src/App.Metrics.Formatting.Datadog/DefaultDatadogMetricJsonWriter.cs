// <copyright file="DefaultHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatting.Datadog.Internal;

namespace App.Metrics.Formatting.Datadog
{
    public class DefaultDatadogMetricJsonWriter : IDatadogMetricJsonWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "mtype" };

        /// <inheritdoc />
        public async Task Write(Utf8JsonWriter jsonWriter, DatadogPoint point, bool writeTimestamp = true)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            var tagsDictionary = point.Tags.ToDictionary();

            var tags = tagsDictionary.Where(tag => !ExcludeTags.Contains(tag.Key));

            var utcTimestamp = point.UtcTimestamp ?? DateTime.UtcNow;

            tagsDictionary.TryGetValue("mtype", out var metricType);
            
            foreach (var f in point.Fields)
            {
                jsonWriter.WriteStartObject();

                var hasPrevious = false;
                var metricWriter = new StringWriter();

                if (!string.IsNullOrWhiteSpace(point.Context))
                {
                    await metricWriter.WriteAsync(point.Context);
                    hasPrevious = true;
                }

                if (hasPrevious)
                {
                    await metricWriter.WriteAsync(".");
                }

                await metricWriter.WriteAsync(point.Measurement.Replace(' ', '_'));

                if (!string.IsNullOrWhiteSpace(metricType))
                {
                    await metricWriter.WriteAsync(".");
                    await metricWriter.WriteAsync(metricType);
                }
                
                await metricWriter.WriteAsync(".");
                await metricWriter.WriteAsync(f.Key);

                var metric = metricWriter.ToString();

                jsonWriter.WriteString("metric", metric);
                
                jsonWriter.WriteStartArray("points");
                
                jsonWriter.WriteStartArray();
                jsonWriter.WriteNumberValue(DatadogSyntax.FormatTimestamp(utcTimestamp));
                jsonWriter.WriteNumberValue(DatadogSyntax.FormatValue(f.Value, metric));
                jsonWriter.WriteEndArray();
                
                jsonWriter.WriteEndArray();

                var datadogMetricType = DatadogSyntax.FormatMetricType(metricType);
                jsonWriter.WriteString("type", datadogMetricType);

                if (datadogMetricType == DatadogSyntax.Rate || datadogMetricType == DatadogSyntax.Count)
                {
                    jsonWriter.WriteNumber("interval", point.FlushInterval.Seconds);
                }

                jsonWriter.WritePropertyName("tags");
                jsonWriter.WriteStartArray();

                foreach (var tag in tags)
                {
                    jsonWriter.WriteStringValue($"{tag.Key}:{tag.Value}");
                }

                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();
            }
        }
    }
}