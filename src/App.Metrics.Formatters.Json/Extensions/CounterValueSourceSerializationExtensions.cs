// <copyright file="CounterValueSourceSerializationExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Formatters.Json;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Counter
    // ReSharper restore CheckNamespace
{
    public static class CounterValueSourceSerializationExtensions
    {
        public static IEnumerable<CounterMetric> ToSerializableMetric(this IEnumerable<CounterValueSource> source) { return source.Select(x => x.ToSerializableMetric()); }

        public static CounterMetric ToSerializableMetric(this CounterValueSource source)
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
                                                  }).
                                      ToArray(),
                       Tags = source.Tags.ToDictionary()
                   };
        }

        public static CounterValueSource FromSerializableMetric(this CounterMetric source)
        {
            var items = source.Items.Select(i => new CounterValue.SetItem(i.Item, i.Count, i.Percent)).ToArray();
            var counterValue = new CounterValue(source.Count, items);

            return new CounterValueSource(
                source.Name,
                ConstantValue.Provider(counterValue),
                source.Unit,
                source.Tags.FromDictionary());
        }

        public static IEnumerable<CounterValueSource> FromSerializableMetric(this IEnumerable<CounterMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }
    }
}