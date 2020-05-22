// <copyright file="DefaultGraphitePointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Graphite.Internal
{
    public class DefaultGraphitePointTextWriter : IGraphitePointTextWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "app", "env", "server", "mtype", "unit", "unit_rate", "unit_dur" };

        /// <inheritdoc />
        public async ValueTask WriteAsync(TextWriter textWriter, GraphitePoint point, bool writeTimestamp = true)
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

            if (point.Context.IsPresent())
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
                await textWriter.WriteAsync(prefix);
                await textWriter.WriteAsync(GraphiteSyntax.EscapeName(f.Key));

                await textWriter.WriteAsync(' ');
                await textWriter.WriteAsync(GraphiteSyntax.FormatValue(f.Value));

                await textWriter.WriteAsync(' ');
                await textWriter.WriteAsync(GraphiteSyntax.FormatTimestamp(utcTimestamp));

                await textWriter.WriteAsync('\n');
            }
        }
    }
}