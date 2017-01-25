// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;

// ReSharper disable CheckNamespace
namespace App.Metrics.Gauge
    // ReSharper restore CheckNamespace
{
    public static class GaugeExtensions
    {
        public static IEnumerable<GaugeMetric> ToMetric(this IEnumerable<GaugeValueSource> source) { return source.Select(ToMetric); }

        public static GaugeMetric ToMetric(this GaugeValueSource source)
        {
            return new GaugeMetric
                   {
                       Name = source.Name,
                       Value = source.Value,
                       Unit = source.Unit.Name,
                       Tags = source.Tags
                   };
        }

        public static GaugeValueSource ToMetricValueSource(this GaugeMetric source)
        {
            return source.Value.HasValue
                ? new GaugeValueSource(source.Name, ConstantValue.Provider(source.Value.Value), source.Unit, source.Tags)
                : new GaugeValueSource(source.Name, null, source.Unit, source.Tags);
        }

        public static IEnumerable<GaugeValueSource> ToMetricValueSource(this IEnumerable<GaugeMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}