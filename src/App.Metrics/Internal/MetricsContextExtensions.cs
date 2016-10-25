// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.MetricData;

// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        public static async Task<CounterValue> CounterValueAsync(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            var data = await metricsContext.GetDataForAsync(groupName);
            return ValueFor(data.Counters, groupName, metricName);
        }

        public static async Task<double> GaugeValueAsync(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            var data = await metricsContext.GetDataForAsync(groupName);
            return ValueFor(data.Gauges, groupName, metricName);
        }

        public static async Task<MetricsDataGroup> GetDataForAsync(this IMetricsContext metricsContext, string groupName)
        {
            var data = await metricsContext.Advanced.DataManager.GetMetricsDataAsync();

            if (data.Groups.Any(m => m.GroupName == groupName))
            {
                return data.Groups.Single(m => m.GroupName == groupName);
            }

            return MetricsDataGroup.Empty;
        }

        public static async Task<HistogramValue> HistogramValueAsync(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            var data = await metricsContext.GetDataForAsync(groupName);

            return ValueFor(data.Histograms, groupName, metricName);
        }

        public static async Task<MeterValue> MeterValueAsync(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            var data = await metricsContext.GetDataForAsync(groupName);

            return ValueFor(data.Meters, groupName, metricName);
        }

        public static async Task<TimerValue> TimerValueAsync(this IMetricsContext metricsContext, string groupName, string metricName)
        {
            var data = await metricsContext.GetDataForAsync(groupName);

            return ValueFor(data.Timers, groupName, metricName);
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