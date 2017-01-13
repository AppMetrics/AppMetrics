// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Extensions
{
    public static class MeterExtensions
    {
        public static IEnumerable<Meter> ToMetric(this IEnumerable<MeterValueSource> source) { return source.Select(x => x.ToMetric()); }

        public static Meter ToMetric(this MeterValueSource source)
        {
            var items = source.Value.Items.Select(
                i =>
                    new Meter.SetItem
                    {
                        Count = i.Value.Count,
                        OneMinuteRate = i.Value.OneMinuteRate,
                        FiveMinuteRate = i.Value.FiveMinuteRate,
                        FifteenMinuteRate = i.Value.FifteenMinuteRate,
                        MeanRate = i.Value.MeanRate,
                        Item = i.Item,
                        Percent = i.Percent
                    }).ToArray();

            return new Meter
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
                       Tags = source.Tags
                   };
        }

        public static MeterValueSource ToMetricValueSource(this Meter source)
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
                }).ToArray();

            var meterValue = new MeterValue(
                source.Count,
                source.MeanRate,
                source.OneMinuteRate,
                source.FiveMinuteRate,
                source.FifteenMinuteRate,
                rateUnit,
                items);

            return new MeterValueSource(source.Name, ConstantValue.Provider(meterValue), source.Unit, rateUnit, source.Tags);
        }

        public static IEnumerable<MeterValueSource> ToMetricValueSource(this IEnumerable<Meter> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}