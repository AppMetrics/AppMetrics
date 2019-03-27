// <copyright file="DefaultHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public class DefaultHostedMetricsPointTextWriter : IHostedMetricsPointTextWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "app", "env", "server", "mtype", "unit", "unit_rate", "unit_dur" };

        /// <inheritdoc />
        public void Write(JsonWriter textWriter, HostedMetricsPoint point, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var hasPrevious = false;
            var measurementWriter = new StringWriter();

            var tagsDictionary = point.Tags.ToDictionary(GraphiteSyntax.EscapeName);

            if (tagsDictionary.TryGetValue("app", out var appValue))
            {
                measurementWriter.Write("app.");
                measurementWriter.Write(appValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("env", out var envValue))
            {
                if (hasPrevious)
                {
                    measurementWriter.Write(".");
                }

                measurementWriter.Write("env.");
                measurementWriter.Write(envValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("server", out var serverValue))
            {
                if (hasPrevious)
                {
                    measurementWriter.Write(".");
                }

                measurementWriter.Write("server.");
                measurementWriter.Write(serverValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("mtype", out var metricType) && !string.IsNullOrWhiteSpace(metricType))
            {
                if (hasPrevious)
                {
                    measurementWriter.Write(".");
                }

                measurementWriter.Write(metricType);
                hasPrevious = true;
            }

            if (!string.IsNullOrWhiteSpace(point.Context))
            {
                if (hasPrevious)
                {
                    measurementWriter.Write(".");
                }

                measurementWriter.Write(GraphiteSyntax.EscapeName(point.Context, true));
                hasPrevious = true;
            }

            if (hasPrevious)
            {
                measurementWriter.Write(".");
            }

            measurementWriter.Write(GraphiteSyntax.EscapeName(point.Measurement, true));

            var tags = tagsDictionary.Where(tag => !ExcludeTags.Contains(tag.Key));

            foreach (var tag in tags)
            {
                measurementWriter.Write('.');
                measurementWriter.Write(GraphiteSyntax.EscapeName(tag.Key));
                measurementWriter.Write('.');
                measurementWriter.Write(tag.Value);
            }

            measurementWriter.Write('.');

            var prefix = measurementWriter.ToString();

            var utcTimestamp = point.UtcTimestamp ?? DateTime.UtcNow;

            foreach (var f in point.Fields)
            {
                textWriter.WriteStartObject();

                var metric = $"{prefix}{GraphiteSyntax.EscapeName(f.Key)}";

                textWriter.WritePropertyName("name");
                // in graphite style format. should be same as Metric field below (used for partitioning, schema matching, indexing)
                textWriter.WriteValue(metric);

                textWriter.WritePropertyName("metric");
                // in graphite style format. should be same as Name field above (used to generate Id)
                textWriter.WriteValue(metric);

                textWriter.WritePropertyName("value");
                textWriter.WriteValue(HostedMetricsSyntax.FormatValue(f.Value, metric));

                textWriter.WritePropertyName("interval");
                textWriter.WriteValue(point.FlushInterval.Seconds);

                textWriter.WritePropertyName("mtype");
                // not used yet. but should be one of gauge rate count counter timestamp
                textWriter.WriteValue("gauge");

                textWriter.WritePropertyName("unit");
                // not needed or used yet
                textWriter.WriteValue(string.Empty);

                textWriter.WritePropertyName("time");
                // unix timestamp in seconds
                textWriter.WriteValue(HostedMetricsSyntax.FormatTimestamp(utcTimestamp));

                textWriter.WritePropertyName("tags");
                textWriter.WriteStartArray();

                // TODO: Hosted Metrics requests take tags but not yet used, provided in metric tag for now.

                textWriter.WriteEndArray();
                textWriter.WriteEndObject();
            }
        }
    }
}