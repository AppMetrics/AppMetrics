// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Formatters.Json;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Gauge
    // ReSharper restore CheckNamespace
{
    public static class GaugeValueSourceSerializationExtensions
    {
        public static GaugeValueSource FromSerializableMetric(this GaugeMetric source)
        {
            var tags = source.Tags.FromDictionary();
            return source.Value.HasValue
                ? new GaugeValueSource(source.Name, source.Group, ConstantValue.Provider(source.Value.Value), source.Unit, tags)
                : new GaugeValueSource(source.Name, source.Group, null, source.Unit, tags);
        }

        public static IEnumerable<GaugeValueSource> FromSerializableMetric(this IEnumerable<GaugeMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<GaugeMetric> ToSerializableMetric(this IEnumerable<GaugeValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static GaugeMetric ToSerializableMetric(this GaugeValueSource source)
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
    }
}