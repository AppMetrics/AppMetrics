// <copyright file="MeterValueSourceSerializationExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Formatters.Json;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Meter
    // ReSharper restore CheckNamespace
{
    public static class MeterValueSourceSerializationExtensions
    {
        public static MeterValueSource FromSerializableMetric(this MeterMetric source)
        {
            var rateUnit = source.RateUnit.FromUnit();
            var items = source.Items.Select(
                                   i =>
                                   {
                                       var value = new MeterValue(
                                           i.Count,
                                           i.MeanRate,
                                           i.OneMinuteRate,
                                           i.FiveMinuteRate,
                                           i.FifteenMinuteRate,
                                           rateUnit);

                                       return new MeterValue.SetItem(
                                           i.Item,
                                           i.Percent,
                                           value);
                                   }).
                               ToArray();

            var meterValue = new MeterValue(
                source.Count,
                source.MeanRate,
                source.OneMinuteRate,
                source.FiveMinuteRate,
                source.FifteenMinuteRate,
                rateUnit,
                items);

            return new MeterValueSource(
                source.Name,
                ConstantValue.Provider(meterValue),
                source.Unit,
                rateUnit,
                source.Tags.FromDictionary());
        }

        public static IEnumerable<MeterValueSource> FromSerializableMetric(this IEnumerable<MeterMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<MeterMetric> ToSerializableMetric(this IEnumerable<MeterValueSource> source)
        {
            return source.Select(x => x.ToSerializableMetric());
        }

        public static MeterMetric ToSerializableMetric(this MeterValueSource source)
        {
            var items = source.Value.Items.Select(
                                   i =>
                                       new MeterMetric.SetItem
                                       {
                                           Count = i.Value.Count,
                                           OneMinuteRate = i.Value.OneMinuteRate,
                                           FiveMinuteRate = i.Value.FiveMinuteRate,
                                           FifteenMinuteRate = i.Value.FifteenMinuteRate,
                                           MeanRate = i.Value.MeanRate,
                                           Item = i.Item,
                                           Percent = i.Percent
                                       }).
                               ToArray();

            return new MeterMetric
                   {
                       RateUnit = source.Value.RateUnit.Unit(),
                       Items = items,
                       Count = source.Value.Count,
                       Name = source.Name,
                       Unit = source.Unit.Name,
                       OneMinuteRate = source.Value.OneMinuteRate,
                       FiveMinuteRate = source.Value.FiveMinuteRate,
                       FifteenMinuteRate = source.Value.FifteenMinuteRate,
                       MeanRate = source.Value.MeanRate,
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}