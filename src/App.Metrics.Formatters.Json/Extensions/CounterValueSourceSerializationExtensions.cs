// <copyright file="CounterValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Formatters.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Counter
    // ReSharper restore CheckNamespace
{
    public static class CounterValueSourceSerializationExtensions
    {
        public static IEnumerable<CounterMetric> ToSerializableMetric(this IEnumerable<CounterValueSource> source) { return source.Select(x => x.ToSerializableMetric()); }

        public static CounterMetric ToSerializableMetric(this CounterValueSource source)
        {
            var counterValue = source.ValueProvider.GetValue(source.ResetOnReporting);
            IEnumerable<CounterMetric.SetItem> items;

            if (source.ReportSetItems && counterValue.Items.Any())
            {
                items = counterValue.Items.Select(
                                       item => new CounterMetric.SetItem
                                                 {
                                                   Count = item.Count,
                                                   Percent = source.ReportItemPercentages ? item.Percent : default,
                                                   Item = item.Item
                                                 }).
                                     ToArray();
            }
            else
            {
                items = Enumerable.Empty<CounterMetric.SetItem>();
            }

            return new CounterMetric
                     {
                       Name = source.Name,
                       Count = counterValue.Count,
                       Unit = source.Unit.Name,
                       Items = items,
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