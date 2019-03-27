// <copyright file="DefaultGraphitePointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Formatters.Graphite.Internal
{
    public class DefaultGraphitePointTextWriter : IGraphitePointTextWriter
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string> { "app", "env", "server", "mtype", "unit", "unit_rate", "unit_dur" };

        /// <inheritdoc />
        public void Write(TextWriter textWriter, GraphitePoint point, bool writeTimestamp = true)
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

            if (point.Context.IsPresent())
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
                textWriter.Write(prefix);
                textWriter.Write(GraphiteSyntax.EscapeName(f.Key));

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatValue(f.Value));

                textWriter.Write(' ');
                textWriter.Write(GraphiteSyntax.FormatTimestamp(utcTimestamp));

                textWriter.Write('\n');
            }
        }
    }
}