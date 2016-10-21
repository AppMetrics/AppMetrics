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
        public static CounterValue CounterValue(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            return ValueFor(metricsContext.GetDataFor(nameWithContext).Counters, nameWithContext);
        }

        public static double GaugeValue(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            return ValueFor(metricsContext.GetDataFor(nameWithContext).Gauges, nameWithContext);
        }

        public static IMetricsContext GetContextFor(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            if (nameWithContext.Length == 1)
            {
                return metricsContext;
            }

            var context = metricsContext.Advanced.Group(nameWithContext.First());
            return context.GetContextFor(nameWithContext.Skip(1).ToArray());
        }

        public static MetricsData GetDataFor(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            var context = metricsContext.GetContextFor(nameWithContext);
            return context.Advanced.MetricsDataProvider.GetMetricsData(context);
        }

        public static HistogramValue HistogramValue(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            return ValueFor(metricsContext.GetDataFor(nameWithContext).Histograms, nameWithContext);
        }

        public static MeterValue MeterValue(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            return ValueFor(metricsContext.GetDataFor(nameWithContext).Meters, nameWithContext);
        }

        public static TimerValue TimerValue(this IMetricsContext metricsContext, params string[] nameWithContext)
        {
            return ValueFor(metricsContext.GetDataFor(nameWithContext).Timers, nameWithContext);
        }

        private static T ValueFor<T>(IEnumerable<MetricValueSource<T>> values, string[] nameWithContext)
        {
            var metricValueSources = values as MetricValueSource<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == nameWithContext.Last()).Select(t => t.Value).ToList();

            if (value.Any() && value.Count() <= 1) return value.Single();

            var name = nameWithContext.Last();
            var context = string.Join(".", nameWithContext.Take(nameWithContext.Length - 1));
            var availableNames = string.Join(",", metricValueSources.Select(v => v.Name));
            throw new InvalidOperationException($"No metric found with name {name} in context {context}. Available names: {availableNames}");
        }
    }
}