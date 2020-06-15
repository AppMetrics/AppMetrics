// <copyright file="HostedMetricsPoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal
{
    public class HostedMetricsPoint
    {
        private readonly IHostedMetricsPointTextWriter _pointTextWriter;

        public HostedMetricsPoint(
            string context,
            string measurement,
            IReadOnlyDictionary<string, object> fields,
            MetricTags tags,
            IHostedMetricsPointTextWriter pointTextWriter,
            TimeSpan flushInterval,
            DateTime? utcTimestamp = null)
        {
            _pointTextWriter = pointTextWriter ?? throw new ArgumentNullException(nameof(pointTextWriter));

            FlushInterval = flushInterval;

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

        public TimeSpan FlushInterval { get; }

        public DateTime? UtcTimestamp { get; }

        public void Write(Utf8JsonWriter jsonWriter, bool writeTimestamp = true)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException(nameof(jsonWriter));
            }

            _pointTextWriter.Write(jsonWriter, this, writeTimestamp);
        }
    }
}
