// <copyright file="MetricDataSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;

namespace App.Metrics.Formatters.Json.Extensions
{
    public static class MetricDataSerializationExtensions
    {
        public static MetricData ToMetric(this MetricsDataValueSource source)
        {
            var jsonContexts = source.Contexts.ToSerializableMetric();

            return new MetricData
                   {
                       Timestamp = source.Timestamp,
                       Contexts = jsonContexts.ToArray()
                   };
        }

        public static MetricsDataValueSource ToMetricValueSource(this MetricData source)
        {
            var contexts = source.Contexts.FromSerializableMetric();

            return new MetricsDataValueSource(source.Timestamp, contexts);
        }
    }
}