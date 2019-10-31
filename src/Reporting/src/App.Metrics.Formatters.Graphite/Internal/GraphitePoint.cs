// <copyright file="GraphitePoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Graphite.Internal
{
    public class GraphitePoint
    {
        private readonly IGraphitePointTextWriter _pointTextWriter;

        public GraphitePoint(
            string context,
            string measurement,
            IReadOnlyDictionary<string, object> fields,
            MetricTags tags,
            IGraphitePointTextWriter pointTextWriter,
            DateTime? utcTimestamp = null)
        {
            _pointTextWriter = pointTextWriter ?? throw new ArgumentNullException(nameof(pointTextWriter));

            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement must be specified", nameof(measurement));
            }

            if (fields == null || fields.Count == 0)
            {
                throw new ArgumentException("At least one field must be specified", nameof(fields));
            }

            if (fields.Any(f => string.IsNullOrEmpty(f.Key)))
            {
                throw new ArgumentException("Fields must have non-empty names", nameof(fields));
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC", nameof(utcTimestamp));
            }

            Context = context;
            Measurement = measurement;
            Fields = fields;
            Tags = tags;
            UtcTimestamp = utcTimestamp ?? DateTime.UtcNow;
        }

        public string Context { get; }

        public IReadOnlyDictionary<string, object> Fields { get; }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public DateTime? UtcTimestamp { get; }

        public ValueTask WriteAsync(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            return _pointTextWriter.WriteAsync(textWriter, this, writeTimestamp);
        }
    }
}
