// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using App.Metrics.Data;
using App.Metrics.Infrastructure;

namespace App.Metrics.Extensions
{
    public static class MetricDataExtensions
    {
        public static MetricData ToMetric(this MetricsDataValueSource source)
        {
            var jsonContexts = source.Contexts.ToMetric();

            return new MetricData
            {
                Environment = source.Environment.ToEnvDictionary(),
                Timestamp = source.Timestamp,
                Contexts = jsonContexts.ToArray()
            };
        }

        public static MetricsDataValueSource ToMetricValueSource(this MetricData source)
        {
            var contexts = source.Contexts.ToMetricValueSource();

            return new MetricsDataValueSource(source.Timestamp, new EnvironmentInfo(source.Environment), contexts);
        }
    }
}