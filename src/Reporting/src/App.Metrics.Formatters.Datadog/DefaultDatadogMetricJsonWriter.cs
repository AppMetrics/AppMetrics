// <copyright file="DefaultHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatters.Datadog.Internal;

namespace App.Metrics.Formatters.Datadog
{
    public class DefaultDatadogMetricJsonWriter : IDatadogMetricJsonWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "mtype" };

        /// <inheritdoc />
        public Task Write(Utf8JsonWriter jsonWriter, DatadogPoint point, bool writeTimestamp = true)
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

                var metric = $"{point.Context}.{point.Measurement.Replace(' ', '_')}.{metricType}.{f.Key}";

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

            return Task.CompletedTask;
        }
    }
}