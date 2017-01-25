// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

// ReSharper disable CheckNamespace
namespace App.Metrics.Counter
    // ReSharper restore CheckNamespace
{
    public static class CounterExtensions
    {
        public static IEnumerable<CounterMetric> ToMetric(this IEnumerable<CounterValueSource> source) { return source.Select(x => x.ToMetric()); }

        public static CounterMetric ToMetric(this CounterValueSource source)
        {
            return new CounterMetric
                   {
                       Name = source.Name,
                       Count = source.Value.Count,
                       Unit = source.Unit.Name,
                       Items = source.Value.Items.Select(
                           item => new CounterMetric.SetItem
                                   {
                                       Count = item.Count,
                                       Percent = item.Percent,
                                       Item = item.Item
                                   }).ToArray(),
                       Tags = source.Tags
                   };
        }

        public static CounterValueSource ToMetricValueSource(this CounterMetric source)
        {
            var items = source.Items.Select(i => new CounterValue.SetItem(i.Item, i.Count, i.Percent)).ToArray();
            var counterValue = new CounterValue(source.Count, items);
            return new CounterValueSource(source.Name, ConstantValue.Provider(counterValue), source.Unit, source.Tags);
        }

        public static IEnumerable<CounterValueSource> ToMetricValueSource(this IEnumerable<CounterMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}