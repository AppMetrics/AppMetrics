// <copyright file="StatsDPoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Formatting.StatsD.Internal
{
    public class StatsDPoint
    {
        private readonly IStatsDMetricStringSerializer _metricStringSerializer;

        public StatsDPoint(
            string name,
            object value,
            string metricType,
            double? sampleRate,
            Dictionary<string, string> tags,
            IStatsDMetricStringSerializer metricStringSerializer,
            DateTime? utcTimestamp = null)
        {
            _metricStringSerializer = metricStringSerializer ?? throw new ArgumentNullException(nameof(metricStringSerializer));

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must be non-null, non-empty and non-whitespace", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(metricType))
            {
                throw new ArgumentException("Metric type must be defined.", nameof(metricType));
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC", nameof(utcTimestamp));
            }

            Name = name;
            Value = value ?? throw new ArgumentException("A value must be specified", nameof(value));
            MetricType = metricType;
            SampleRate = sampleRate;
            Tags = tags;
            UtcTimestamp = utcTimestamp ?? DateTime.UtcNow;
        }

        public string MetricType { get; }

        public string Name { get; }

        public double? SampleRate { get; }

        public Dictionary<string, string> Tags { get; }

        public DateTime UtcTimestamp { get; }

        public object Value { get; }

        public string Write(MetricsStatsDOptions options)
            => _metricStringSerializer.Serialize(this, options);
    }
}