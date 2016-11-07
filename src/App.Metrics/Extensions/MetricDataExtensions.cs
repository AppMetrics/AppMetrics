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
            var jsonGroups = source.Groups.ToMetric();

            return new MetricData
            {
                ContextName = source.ContextName,
                Environment = source.Environment.ToEnvDictionary(),
                Timestamp = source.Timestamp,
                Version = "1",
                Groups = jsonGroups.ToArray()
            };
        }

        public static MetricsDataValueSource ToMetricValueSource(this MetricData source)
        {
            var groups = source.Groups.ToMetricValueSource();

            return new MetricsDataValueSource(source.ContextName, source.Timestamp, new EnvironmentInfo(source.Environment), groups);
        }
    }
}