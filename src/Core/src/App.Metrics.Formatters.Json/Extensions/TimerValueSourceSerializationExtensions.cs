// <copyright file="TimerValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
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
        public static BucketTimerValueSource FromSerializableMetric(this BucketTimerMetric source)
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

            var histogramValue = new BucketHistogramValue(
                source.Count,
                source.Histogram.Sum,
                new ReadOnlyDictionary<double, double>(source.Histogram.Buckets));

            var timerValue = new BucketTimerValue(rateValue, histogramValue, source.ActiveSessions, durationUnit);

            return new BucketTimerValueSource(
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

        public static IEnumerable<BucketTimerValueSource> FromSerializableMetric(this IEnumerable<BucketTimerMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<TimerMetric> ToSerializableMetric(this IEnumerable<TimerValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static IEnumerable<BucketTimerMetric> ToSerializableMetric(this IEnumerable<BucketTimerValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static TimerMetric ToSerializableMetric(this TimerValueSource source)
        {
            var histogramData = new TimerMetric.HistogramData
                                {
                                    Sum = source.Value.Histogram.Sum,
                                    LastValue = source.Value.Histogram.LastValue,
                                    LastUserValue = source.Value.Histogram.LastUserValue,
                                    Max = source.Value.Histogram.Max,
                                    MaxUserValue = source.Value.Histogram.MaxUserValue,
                                    Mean = source.Value.Histogram.Mean,
                                    Min = source.Value.Histogram.Min,
                                    MinUserValue = source.Value.Histogram.MinUserValue,
                                    StdDev = source.Value.Histogram.StdDev,
                                    Median = source.Value.Histogram.Median,
                                    Percentile75 = source.Value.Histogram.Percentile75,
                                    Percentile95 = source.Value.Histogram.Percentile95,
                                    Percentile98 = source.Value.Histogram.Percentile98,
                                    Percentile99 = source.Value.Histogram.Percentile99,
                                    Percentile999 = source.Value.Histogram.Percentile999,
                                    SampleSize = source.Value.Histogram.SampleSize,
                                };

            var rateData = new TimerMetric.RateData
                           {
                               MeanRate = source.Value.Rate.MeanRate,
                               OneMinuteRate = source.Value.Rate.OneMinuteRate,
                               FiveMinuteRate = source.Value.Rate.FiveMinuteRate,
                               FifteenMinuteRate = source.Value.Rate.FifteenMinuteRate
                           };

            return new TimerMetric
                   {
                       Name = source.Name,
                       Count = source.Value.Rate.Count,
                       ActiveSessions = source.Value.ActiveSessions,
                       Rate = rateData,
                       Histogram = histogramData,
                       Unit = source.Unit.Name,
                       RateUnit = source.RateUnit.Unit(),
                       DurationUnit = source.DurationUnit.Unit(),
                       Tags = source.Tags.ToDictionary()
                   };
        }

        public static BucketTimerMetric ToSerializableMetric(this BucketTimerValueSource source)
        {
            var histogramData = new BucketTimerMetric.BucketHistogramData
            {
                Sum = source.Value.Histogram.Sum,
                Buckets = source.Value.Histogram.Buckets.ToDictionary(x=>x.Key, x=>x.Value)
            };

            var rateData = new BucketTimerMetric.RateData
            {
                MeanRate = source.Value.Rate.MeanRate,
                OneMinuteRate = source.Value.Rate.OneMinuteRate,
                FiveMinuteRate = source.Value.Rate.FiveMinuteRate,
                FifteenMinuteRate = source.Value.Rate.FifteenMinuteRate
            };

            return new BucketTimerMetric
            {
                Name = source.Name,
                Count = source.Value.Rate.Count,
                ActiveSessions = source.Value.ActiveSessions,
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