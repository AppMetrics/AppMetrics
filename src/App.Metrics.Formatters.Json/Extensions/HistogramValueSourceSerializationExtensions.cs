// <copyright file="HistogramValueSourceSerializationExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<HistogramValueSource> FromSerializableMetric(this IEnumerable<HistogramMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<HistogramMetric> ToSerializableMetric(this IEnumerable<HistogramValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static HistogramMetric ToSerializableMetric(this HistogramValueSource source)
        {
            return new HistogramMetric
                   {
                       Name = source.Name,
                       Count = source.Value.Count,
                       Sum = source.Value.Sum,
                       Unit = source.Unit.Name,
                       LastUserValue = source.Value.LastUserValue,
                       LastValue = source.Value.LastValue,
                       Max = source.Value.Max,
                       MaxUserValue = source.Value.MaxUserValue,
                       Mean = source.Value.Mean,
                       Median = source.Value.Median,
                       Min = source.Value.Min,
                       MinUserValue = source.Value.MinUserValue,
                       Percentile75 = source.Value.Percentile75,
                       Percentile95 = source.Value.Percentile95,
                       Percentile98 = source.Value.Percentile98,
                       Percentile99 = source.Value.Percentile99,
                       Percentile999 = source.Value.Percentile999,
                       SampleSize = source.Value.SampleSize,
                       StdDev = source.Value.StdDev,
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}