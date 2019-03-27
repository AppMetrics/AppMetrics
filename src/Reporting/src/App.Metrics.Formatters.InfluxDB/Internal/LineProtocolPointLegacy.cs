// <copyright file="LineProtocolPointLegacy.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    /// This class is not used anymore and is here only for baseline benchmarks.
    /// </summary>
    [Obsolete("This class is not used anymore and should be replaced by LineProtocolMultipleValues or LineProtocolSingleValue when appropriate")]
    internal class LineProtocolPointLegacy : ILineProtocolPoint
    {
        public LineProtocolPointLegacy(
            string measurement,
            IEnumerable<KeyValuePair<string, object>> fields,
            MetricTags tags,
            DateTime? utcTimestamp = null)
        {
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement name must be specified");
            }

            if (fields == null || !fields.Any())
            {
                throw new ArgumentException("At least one field must be specified");
            }

            if (fields.Any(f => string.IsNullOrEmpty(f.Key)))
            {
                throw new ArgumentException("Fields must have non-empty names");
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC");
            }

            Measurement = measurement;
            Fields = fields;
            Tags = tags;
            UtcTimestamp = utcTimestamp;
        }

        public IEnumerable<KeyValuePair<string, object>> Fields { get; }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public DateTime? UtcTimestamp { get; }

        public void Write(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            textWriter.Write(LineProtocolSyntax.EscapeName(Measurement));

            if (Tags.Count > 0)
            {
                for (var i = 0; i < Tags.Count; i++)
                {
                    textWriter.Write(',');
                    textWriter.Write(LineProtocolSyntax.EscapeName(Tags.Keys[i]));
                    textWriter.Write('=');
                    textWriter.Write(LineProtocolSyntax.EscapeName(Tags.Values[i]));
                }
            }

            var fieldDelim = ' ';

            foreach (var f in Fields)
            {
                textWriter.Write(fieldDelim);
                fieldDelim = ',';
                textWriter.Write(LineProtocolSyntax.EscapeName(f.Key));
                textWriter.Write('=');
                textWriter.Write(LineProtocolSyntax.FormatValue(f.Value));
            }

            if (!writeTimestamp)
            {
                return;
            }

            textWriter.Write(' ');

            if (UtcTimestamp == null)
            {
                textWriter.Write(LineProtocolSyntax.FormatTimestamp(DateTime.UtcNow));
                return;
            }

            textWriter.Write(LineProtocolSyntax.FormatTimestamp(UtcTimestamp.Value));
        }
    }
}