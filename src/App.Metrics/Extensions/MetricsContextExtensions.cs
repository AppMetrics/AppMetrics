// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

// ReSharper disable CheckNamespace

namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        public static CounterValue CounterValue(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            return ValueFor(metricsContext.GetDataFor(groupName).Counters, groupName, metricName);
        }

        public static double GaugeValue(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            return ValueFor(metricsContext.GetDataFor(groupName).Gauges, groupName, metricName);
        }

        public static MetricsData GetDataFor(this IMetricsContext metricsContext, string groupName)
        {
            var data = metricsContext.Advanced.MetricsDataManager.GetMetricsData();

            if (data.Context == groupName)
            {
                return data;
            }

            if (data.ChildMetrics.Any(m => m.Context == groupName))
            {
                return data.ChildMetrics.First(m => m.Context == groupName);
            }

            return MetricsData.Empty;
        }

        public static HistogramValue HistogramValue(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            return ValueFor(metricsContext.GetDataFor(groupName).Histograms, groupName, metricName);
        }

        public static MeterValue MeterValue(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            return ValueFor(metricsContext.GetDataFor(groupName).Meters, groupName, metricName);
        }

        public static TimerValue TimerValue(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            return ValueFor(metricsContext.GetDataFor(groupName).Timers, groupName, metricName);
        }

        private static T ValueFor<T>(IEnumerable<MetricValueSource<T>> values, string groupName, string metricName)
        {
            var metricValueSources = values as MetricValueSource<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == metricName).Select(t => t.Value).ToList();

            if (value.Any() && value.Count() <= 1) return value.Single();

            var availableNames = string.Join(",", metricValueSources.Select(v => v.Name));
            throw new InvalidOperationException($"No metric found with name {metricName} in group {groupName} Available names: {availableNames}");
        }
    }
}