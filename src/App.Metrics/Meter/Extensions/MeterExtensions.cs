// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;

namespace App.Metrics.Meter.Extensions
{
    public static class MeterExtensions
    {
        public static IEnumerable<MeterMetric> ToMetric(this IEnumerable<MeterValueSource> source) { return source.Select(x => x.ToMetric()); }

        public static MeterMetric ToMetric(this MeterValueSource source)
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
                    }).ToArray();

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
                       Tags = source.Tags
                   };
        }

        public static MeterValueSource ToMetricValueSource(this MeterMetric source)
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

        public static IEnumerable<MeterValueSource> ToMetricValueSource(this IEnumerable<MeterMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}