// <copyright file="GaugeValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Formatters.Json;

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
                ? new GaugeValueSource(source.Name, ConstantValue.Provider(source.Value.Value), source.Unit, tags)
                : new GaugeValueSource(source.Name, null, source.Unit, tags);
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
            var gaugeValue = source.ValueProvider.GetValue(source.ResetOnReporting);
            return new GaugeMetric
                   {
                       Name = source.Name,
                       Value = gaugeValue,
                       Unit = source.Unit.Name,
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}
