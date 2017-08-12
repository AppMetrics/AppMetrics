// <copyright file="MetricsTextPoint.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void Write(TextWriter textWriter, string separator, int padding)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            textWriter.Write($"# TIMESTAMP: {_timestamp.Ticks}");
            textWriter.Write('\n');

            textWriter.Write("# MEASUREMENT: ");
            textWriter.Write(Measurement);
            textWriter.Write('\n');

            if (Tags.Count > 0)
            {
                textWriter.Write("# TAGS:\n");

                for (var i = 0; i < Tags.Count; i++)
                {
                    textWriter.Write(MetricsTextSyntax.FormatReadable(Tags.Keys[i], MetricsTextSyntax.FormatValue(Tags.Values[i]), separator, padding));
                    textWriter.Write('\n');
                }
            }

            textWriter.Write("# FIELDS:\n");

            foreach (var f in Fields)
            {
                textWriter.Write(MetricsTextSyntax.FormatReadable(f.Key, MetricsTextSyntax.FormatValue(f.Value), separator, padding));
                textWriter.Write('\n');
            }

            textWriter.Write("--------------------------------------------------------------");
        }
    }
}