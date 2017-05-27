// <copyright file="AsciiMetricPoint.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Tagging;

namespace App.Metrics.Formatting.Ascii
{
    // TODO: Remove in 2.0.0 - Add to separate formatting package
    public class AsciiMetricPoint
    {
        public AsciiMetricPoint(
            string measurement,
            IReadOnlyDictionary<string, object> fields,
            MetricTags tags)
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

            Measurement = measurement;
            Fields = fields;
            Tags = tags;
        }

        public IReadOnlyDictionary<string, object> Fields { get; }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            textWriter.Write("# MEASUREMENT: ");
            textWriter.Write(Measurement);
            textWriter.Write('\n');

            if (Tags.Count > 0)
            {
                textWriter.Write("# TAGS:\n");

                for (var i = 0; i < Tags.Count; i++)
                {
                    textWriter.Write(AsciiSyntax.FormatReadable(Tags.Keys[i], AsciiSyntax.FormatValue(Tags.Values[i])));
                    textWriter.Write('\n');
                }
            }

            textWriter.Write("# FIELDS:\n");

            foreach (var f in Fields)
            {
                textWriter.Write(AsciiSyntax.FormatReadable(f.Key, AsciiSyntax.FormatValue(f.Value)));
                textWriter.Write('\n');
            }

            textWriter.Write("--------------------------------------------------------------");
        }
    }
}
