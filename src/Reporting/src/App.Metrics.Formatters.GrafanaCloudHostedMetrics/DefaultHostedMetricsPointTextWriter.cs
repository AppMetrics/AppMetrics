// <copyright file="DefaultHostedMetricsPointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics
{
    public class DefaultHostedMetricsPointTextWriter : IHostedMetricsPointTextWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "app", "env", "server", "mtype", "unit", "unit_rate", "unit_dur" };

        /// <inheritdoc />
        public async Task Write(JsonWriter textWriter, HostedMetricsPoint point, bool writeTimestamp = true)
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
                await measurementWriter.WriteAsync("app.");
                await measurementWriter.WriteAsync(appValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("env", out var envValue))
            {
                if (hasPrevious)
                {
                    await measurementWriter.WriteAsync(".");
                }

                await measurementWriter.WriteAsync("env.");
                await measurementWriter.WriteAsync(envValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("server", out var serverValue))
            {
                if (hasPrevious)
                {
                    await measurementWriter.WriteAsync(".");
                }

                await measurementWriter.WriteAsync("server.");
                await measurementWriter.WriteAsync(serverValue);
                hasPrevious = true;
            }

            if (tagsDictionary.TryGetValue("mtype", out var metricType) && !string.IsNullOrWhiteSpace(metricType))
            {
                if (hasPrevious)
                {
                    await measurementWriter.WriteAsync(".");
                }

                await measurementWriter.WriteAsync(metricType);
                hasPrevious = true;
            }

            if (!string.IsNullOrWhiteSpace(point.Context))
            {
                if (hasPrevious)
                {
                    await measurementWriter.WriteAsync(".");
                }

                await measurementWriter.WriteAsync(GraphiteSyntax.EscapeName(point.Context, true));
                hasPrevious = true;
            }

            if (hasPrevious)
            {
                await measurementWriter.WriteAsync(".");
            }

            await measurementWriter.WriteAsync(GraphiteSyntax.EscapeName(point.Measurement, true));

            var tags = tagsDictionary.Where(tag => !ExcludeTags.Contains(tag.Key));

            foreach (var tag in tags)
            {
                await measurementWriter.WriteAsync('.');
                await measurementWriter.WriteAsync(GraphiteSyntax.EscapeName(tag.Key));
                await measurementWriter.WriteAsync('.');
                await measurementWriter.WriteAsync(tag.Value);
            }

            await measurementWriter.WriteAsync('.');

            var prefix = measurementWriter.ToString();

            var utcTimestamp = point.UtcTimestamp ?? DateTime.UtcNow;

            foreach (var f in point.Fields)
            {
                textWriter.WriteStartObject();

                var metric = $"{prefix}{GraphiteSyntax.EscapeName(f.Key)}";

                await textWriter.WritePropertyNameAsync("name");
                // in graphite style format. should be same as Metric field below (used for partitioning, schema matching, indexing)
                await textWriter.WriteValueAsync(metric);

                await textWriter.WritePropertyNameAsync("metric");
                // in graphite style format. should be same as Name field above (used to generate Id)
                await textWriter.WriteValueAsync(metric);

                await textWriter.WritePropertyNameAsync("value");
                await textWriter.WriteValueAsync(HostedMetricsSyntax.FormatValue(f.Value, metric));

                await textWriter.WritePropertyNameAsync("interval");
                await textWriter.WriteValueAsync(point.FlushInterval.Seconds);

                await textWriter.WritePropertyNameAsync("mtype");
                // not used yet. but should be one of gauge rate count counter timestamp
                await textWriter.WriteValueAsync("gauge");

                await textWriter.WritePropertyNameAsync("unit");
                // not needed or used yet
                await textWriter.WriteValueAsync(string.Empty);

                await textWriter.WritePropertyNameAsync("time");
                // unix timestamp in seconds
                await textWriter.WriteValueAsync(HostedMetricsSyntax.FormatTimestamp(utcTimestamp));

                await textWriter.WritePropertyNameAsync("tags");
                await textWriter.WriteStartArrayAsync();

                // TODO: Hosted Metrics requests take tags but not yet used, provided in metric tag for now.

                await textWriter.WriteEndArrayAsync();
                await textWriter.WriteEndObjectAsync();
            }
        }
    }
}