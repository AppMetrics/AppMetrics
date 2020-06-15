// <copyright file="ApdexValueSourceSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Formatters.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Apdex
    // ReSharper restore CheckNamespace
{
    public static class ApdexValueSourceSerializationExtensions
    {
        public static ApdexValueSource FromSerializableMetric(this ApdexMetric source)
        {
            var counterValue = new ApdexValue(source.Score, source.Satisfied, source.Tolerating, source.Frustrating, source.SampleSize);
            return new ApdexValueSource(source.Name, ConstantValue.Provider(counterValue), source.Tags.FromDictionary());
        }

        public static IEnumerable<ApdexValueSource> FromSerializableMetric(this IEnumerable<ApdexMetric> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static IEnumerable<ApdexMetric> ToSerializableMetric(this IEnumerable<ApdexValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        public static ApdexMetric ToSerializableMetric(this ApdexValueSource source)
        {
            var apdexValue = source.ValueProvider.GetValue(source.ResetOnReporting);
            return new ApdexMetric
                   {
                       Name = source.Name,
                       Score = apdexValue.Score,
                       SampleSize = apdexValue.SampleSize,
                       Satisfied = apdexValue.Satisfied,
                       Tolerating = apdexValue.Tolerating,
                       Frustrating = apdexValue.Frustrating,
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}
