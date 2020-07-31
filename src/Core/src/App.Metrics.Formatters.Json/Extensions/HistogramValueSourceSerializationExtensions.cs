// <copyright file="HistogramValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App.Metrics.BucketHistogram;
using App.Metrics.Formatters.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Histogram
// ReSharper restore CheckNamespace
{
    public static class HistogramValueSourceSerializationExtensions
    {
        public static HistogramValueSource FromSerializableMetric(this HistogramMetric source)
        {
            var histogramValue = new HistogramValue(
                source.Count,
                source.Sum,
                source.LastValue,
                source.LastUserValue,
                source.Max,
                source.MaxUserValue,
                source.Mean,
                source.Min,
                source.MinUserValue,
                source.StdDev,
                source.Median,
                source.Percentile75,
                source.Percentile95,
                source.Percentile98,
                source.Percentile99,
                source.Percentile999,
                source.SampleSize);

            return new HistogramValueSource(
                source.Name,
                ConstantValue.Provider(histogramValue),
                source.Unit,
                source.Tags.FromDictionary());
        }

        public static BucketHistogramValueSource FromSerializableMetric(this BucketHistogramMetric source)
        {
            var histogramValue = new BucketHistogramValue(
                source.Count,
                source.Sum,
                new ReadOnlyDictionary<double, double>(source.Buckets));

            return new BucketHistogramValueSource(
                source.Name,
                ConstantValue.Provider(histogramValue),
                source.Unit,
                source.Tags.FromDictionary());
        }

        public static IEnumerable<HistogramValueSource> FromSerializableMetric(this IEnumerable<HistogramMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<BucketHistogramValueSource> FromSerializableMetric(this IEnumerable<BucketHistogramMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<HistogramMetric> ToSerializableMetric(this IEnumerable<HistogramValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static IEnumerable<BucketHistogramMetric> ToSerializableMetric(this IEnumerable<BucketHistogramValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static HistogramMetric ToSerializableMetric(this HistogramValueSource source)
        {
            var histogramValue = source.ValueProvider.GetValue(source.ResetOnReporting);
            return new HistogramMetric
            {
                Name = source.Name,
                Count = histogramValue.Count,
                Sum = histogramValue.Sum,
                Unit = source.Unit.Name,
                LastUserValue = histogramValue.LastUserValue,
                LastValue = histogramValue.LastValue,
                Max = histogramValue.Max,
                MaxUserValue = histogramValue.MaxUserValue,
                Mean = histogramValue.Mean,
                Median = histogramValue.Median,
                Min = histogramValue.Min,
                MinUserValue = histogramValue.MinUserValue,
                Percentile75 = histogramValue.Percentile75,
                Percentile95 = histogramValue.Percentile95,
                Percentile98 = histogramValue.Percentile98,
                Percentile99 = histogramValue.Percentile99,
                Percentile999 = histogramValue.Percentile999,
                SampleSize = histogramValue.SampleSize,
                StdDev = histogramValue.StdDev,
                Tags = source.Tags.ToDictionary()
            };
        }

        public static BucketHistogramMetric ToSerializableMetric(this BucketHistogramValueSource source)
        {
            return new BucketHistogramMetric
            {
                Name = source.Name,
                Count = source.Value.Count,
                Sum = source.Value.Sum,
                Unit = source.Unit.Name,
                Buckets = source.Value.Buckets.ToDictionary(x => x.Key, x => x.Value),
                Tags = source.Tags.ToDictionary()
            };
        }
    }
}
