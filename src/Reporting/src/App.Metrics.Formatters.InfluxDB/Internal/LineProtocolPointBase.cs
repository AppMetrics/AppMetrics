// <copyright file="LineProtocolPointBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    /// Base class for a <see cref="ILineProtocolPoint"/> which takes care of common properties (<see cref="Measurement"/>, <see cref="Tags"/> and <see cref="UtcTimestamp"/>).
    /// </summary>
    internal abstract class LineProtocolPointBase
    {
        public LineProtocolPointBase(string measurement, MetricTags tags, DateTime? utcTimestamp)
        {
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement name must be specified");
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC");
            }

            Measurement = measurement;
            Tags = tags;
            UtcTimestamp = utcTimestamp;
        }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public DateTime? UtcTimestamp { get; }

        /// <summary>
        /// Writes the common properties of the line procol points, which includes writing the measurement name and the different tags.
        /// </summary>
        /// <param name="textWriter">Writer to write the values to.</param>
        protected async ValueTask WriteCommonAsync(TextWriter textWriter)
        {
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
        }

        /// <summary>
        /// Writes the timestamp using the most precise unit.
        /// </summary>
        /// <param name="textWriter">Writer to write the values to.</param>
        protected async ValueTask WriteTimestampAsync(TextWriter textWriter)
        {
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
