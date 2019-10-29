// <copyright file="MetricsTextPoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Ascii.Internal
{
    internal class MetricsTextPoint
    {
        private readonly DateTime _timestamp;

        public MetricsTextPoint(
            string measurement,
            IReadOnlyDictionary<string, object> fields,
            MetricTags tags,
            DateTime timestamp)
        {
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement name must be specified");
            }

            if (fields == null || fields.Count == 0)
            {
                throw new ArgumentException("At least one field must be specified");
            }

            if (fields.Any(f => string.IsNullOrEmpty(f.Key)))
            {
                throw new ArgumentException("Fields must have non-empty names");
            }

            _timestamp = timestamp;

            Measurement = measurement;
            Fields = fields;
            Tags = tags;
        }

        private IReadOnlyDictionary<string, object> Fields { get; }

        private string Measurement { get; }

        private MetricTags Tags { get; }

        public async ValueTask WriteAsync(TextWriter textWriter, string separator, int padding)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            await textWriter.WriteAsync($"# TIMESTAMP: {_timestamp.Ticks}");
            await textWriter.WriteAsync('\n');

            await textWriter.WriteAsync("# MEASUREMENT: ");
            await textWriter.WriteAsync(Measurement);
            await textWriter.WriteAsync('\n');

            if (Tags.Count > 0)
            {
                await textWriter.WriteAsync("# TAGS:\n");

                for (var i = 0; i < Tags.Count; i++)
                {
                    await textWriter.WriteAsync(MetricsTextSyntax.FormatReadable(Tags.Keys[i], MetricsTextSyntax.FormatValue(Tags.Values[i]), separator, padding));
                    await textWriter.WriteAsync('\n');
                }
            }

            await textWriter.WriteAsync("# FIELDS:\n");

            foreach (var f in Fields)
            {
                await textWriter.WriteAsync(MetricsTextSyntax.FormatReadable(f.Key, MetricsTextSyntax.FormatValue(f.Value), separator, padding));
                await textWriter.WriteAsync('\n');
            }

            await textWriter.WriteAsync("--------------------------------------------------------------");
        }
    }
}