// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsDataGroupValueSourceExtensions
    {
        public static CounterValue CounterValueFor(this MetricsDataGroupValueSource valueService, string metricName)
        {
            return valueService.Counters.ValueFor(valueService.GroupName, metricName);
        }

        public static double GaugeValueFor(this MetricsDataGroupValueSource valueService, string metricName)
        {
            return valueService.Gauges.ValueFor(valueService.GroupName, metricName);
        }

        public static HistogramValue HistogramValueFor(this MetricsDataGroupValueSource valueService, string metricName)
        {
            return valueService.Histograms.ValueFor(valueService.GroupName, metricName);
        }

        public static MeterValue MeterValueFor(this MetricsDataGroupValueSource valueService, string metricName)
        {
            return valueService.Meters.ValueFor(valueService.GroupName, metricName);
        }

        public static TimerValue TimerValueFor(this MetricsDataGroupValueSource valueService, string metricName)
        {
            return valueService.Timers.ValueFor(valueService.GroupName, metricName);
        }

        public static T ValueFor<T>(this IEnumerable<MetricValueSource<T>> values, string groupName, string metricName)
        {
            var metricValueSources = values as MetricValueSource<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == metricName).Select(t => t.Value).ToList<T>();

            if (value.Any() && value.Count() <= 1) return value.Single();

            var availableNames = string.Join(",", metricValueSources.Select(v => v.Name));
            throw new InvalidOperationException($"No metric found with name {metricName} in group {groupName} Available names: {availableNames}");
        }
    }
}