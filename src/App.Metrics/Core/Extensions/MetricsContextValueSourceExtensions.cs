// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
    // ReSharper restore CheckNamespace
{
    public static class MetricsContextValueSourceExtensions
    {
        public static ApdexValue ApdexValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.ApdexScores.ValueFor(valueService.Context, metricName);
        }

        public static CounterValue CounterValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.Counters.ValueFor(valueService.Context, metricName);
        }

        public static double GaugeValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.Gauges.ValueFor(valueService.Context, metricName);
        }

        public static HistogramValue HistogramValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.Histograms.ValueFor(valueService.Context, metricName);
        }

        public static MeterValue MeterValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.Meters.ValueFor(valueService.Context, metricName);
        }

        public static TimerValue TimerValueFor(this MetricsContextValueSource valueService, string metricName)
        {
            return valueService.Timers.ValueFor(valueService.Context, metricName);
        }

        public static T ValueFor<T>(this IEnumerable<MetricValueSource<T>> values, string context, string metricName)
        {
            var metricValueSources = values as MetricValueSource<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == metricName).Select(t => t.Value).ToList();

            if (value.Any() && value.Count <= 1)
            {
                return value.Single();
            }

            var availableNames = string.Join(",", metricValueSources.Select(v => v.Name));
            throw new InvalidOperationException($"No metric found with name {metricName} in context {context} Available names: {availableNames}");
        }
    }
}