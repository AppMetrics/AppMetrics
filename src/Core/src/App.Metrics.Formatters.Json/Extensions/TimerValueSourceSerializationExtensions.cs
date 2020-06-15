// <copyright file="TimerValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Formatters.Json;
using App.Metrics.Histogram;
using App.Metrics.Meter;

// ReSharper disable CheckNamespace
namespace App.Metrics.Timer
    // ReSharper restore CheckNamespace
{
    public static class TimerValueSourceSerializationExtensions
    {
        public static TimerValueSource FromSerializableMetric(this TimerMetric source)
        {
            var rateUnit = source.RateUnit.FromUnit();
            var durationUnit = source.DurationUnit.FromUnit();

            var rateValue = new MeterValue(
                source.Count,
                source.Rate.MeanRate,
                source.Rate.OneMinuteRate,
                source.Rate.FiveMinuteRate,
                source.Rate.FifteenMinuteRate,
                rateUnit);

            var histogramValue = new HistogramValue(
                source.Count,
                source.Histogram.Sum,
                source.Histogram.LastValue,
                source.Histogram.LastUserValue,
                source.Histogram.Max,
                source.Histogram.MaxUserValue,
                source.Histogram.Mean,
                source.Histogram.Min,
                source.Histogram.MinUserValue,
                source.Histogram.StdDev,
                source.Histogram.Median,
                source.Histogram.Percentile75,
                source.Histogram.Percentile95,
                source.Histogram.Percentile98,
                source.Histogram.Percentile99,
                source.Histogram.Percentile999,
                source.Histogram.SampleSize);

            var timerValue = new TimerValue(rateValue, histogramValue, source.ActiveSessions, durationUnit);

            return new TimerValueSource(
                source.Name,
                ConstantValue.Provider(timerValue),
                source.Unit,
                rateUnit,
                durationUnit,
                source.Tags.FromDictionary());
        }

        public static IEnumerable<TimerValueSource> FromSerializableMetric(this IEnumerable<TimerMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<TimerMetric> ToSerializableMetric(this IEnumerable<TimerValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static TimerMetric ToSerializableMetric(this TimerValueSource source)
        {
            var timerValue = source.ValueProvider.GetValue(source.ResetOnReporting);
            var histogramData = new TimerMetric.HistogramData
                                {
                                    Sum = timerValue.Histogram.Sum,
                                    LastValue = timerValue.Histogram.LastValue,
                                    LastUserValue = timerValue.Histogram.LastUserValue,
                                    Max = timerValue.Histogram.Max,
                                    MaxUserValue = timerValue.Histogram.MaxUserValue,
                                    Mean = timerValue.Histogram.Mean,
                                    Min = timerValue.Histogram.Min,
                                    MinUserValue = timerValue.Histogram.MinUserValue,
                                    StdDev = timerValue.Histogram.StdDev,
                                    Median = timerValue.Histogram.Median,
                                    Percentile75 = timerValue.Histogram.Percentile75,
                                    Percentile95 = timerValue.Histogram.Percentile95,
                                    Percentile98 = timerValue.Histogram.Percentile98,
                                    Percentile99 = timerValue.Histogram.Percentile99,
                                    Percentile999 = timerValue.Histogram.Percentile999,
                                    SampleSize = timerValue.Histogram.SampleSize,
                                };

            var rateData = new TimerMetric.RateData
                           {
                               MeanRate = timerValue.Rate.MeanRate,
                               OneMinuteRate = timerValue.Rate.OneMinuteRate,
                               FiveMinuteRate = timerValue.Rate.FiveMinuteRate,
                               FifteenMinuteRate = timerValue.Rate.FifteenMinuteRate
                           };

            return new TimerMetric
                   {
                       Name = source.Name,
                       Count = timerValue.Rate.Count,
                       ActiveSessions = timerValue.ActiveSessions,
                       Rate = rateData,
                       Histogram = histogramData,
                       Unit = source.Unit.Name,
                       RateUnit = source.RateUnit.Unit(),
                       DurationUnit = source.DurationUnit.Unit(),
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}
