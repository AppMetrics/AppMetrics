// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Gauge
    // ReSharper restore CheckNamespace
{
    public static class GaugeExtensions
    {
        public static double GetGaugeValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Gauges.ValueFor(context, metricName);
        }

        public static IEnumerable<GaugeMetric> ToMetric(this IEnumerable<GaugeValueSource> source) { return source.Select(ToMetric); }

        public static GaugeMetric ToMetric(this GaugeValueSource source)
        {
            return new GaugeMetric
                   {
                       Name = source.Name,
                       Group = source.Group,
                       Value = source.Value,
                       Unit = source.Unit.Name,
                       Tags = source.Tags.ToDictionary()
                   };
        }

        public static GaugeValueSource ToMetricValueSource(this GaugeMetric source)
        {
            var tags = source.Tags.FromDictionary();
            return source.Value.HasValue
                ? new GaugeValueSource(source.Name, source.Group, ConstantValue.Provider(source.Value.Value), source.Unit, tags)
                : new GaugeValueSource(source.Name, source.Group, null, source.Unit, tags);
        }

        public static IEnumerable<GaugeValueSource> ToMetricValueSource(this IEnumerable<GaugeMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}