// <copyright file="LineProtocolPointLegacy.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async ValueTask WriteAync(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            await textWriter.WriteAsync(LineProtocolSyntax.EscapeName(Measurement));

            if (Tags.Count > 0)
            {
                for (var i = 0; i < Tags.Count; i++)
                {
                    await textWriter.WriteAsync(',');
                    await textWriter.WriteAsync(LineProtocolSyntax.EscapeName(Tags.Keys[i]));
                    await textWriter.WriteAsync('=');
                    await textWriter.WriteAsync(LineProtocolSyntax.EscapeName(Tags.Values[i]));
                }
            }

            var fieldDelim = ' ';

            foreach (var f in Fields)
            {
                await textWriter.WriteAsync(fieldDelim);
                fieldDelim = ',';
                await textWriter.WriteAsync(LineProtocolSyntax.EscapeName(f.Key));
                await textWriter.WriteAsync('=');
                await textWriter.WriteAsync(LineProtocolSyntax.FormatValue(f.Value));
            }

            if (!writeTimestamp)
            {
                return;
            }

            await textWriter.WriteAsync(' ');

            if (UtcTimestamp == null)
            {
                await textWriter.WriteAsync(LineProtocolSyntax.FormatTimestamp(DateTime.UtcNow));
                return;
            }

            await textWriter.WriteAsync(LineProtocolSyntax.FormatTimestamp(UtcTimestamp.Value));
        }
    }
}