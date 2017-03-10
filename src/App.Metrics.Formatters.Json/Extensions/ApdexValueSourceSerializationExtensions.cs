// <copyright file="ApdexValueSourceSerializationExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Formatters.Json;
using App.Metrics.Tagging;

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
            return new ApdexMetric
                   {
                       Name = source.Name,
                       Score = source.Value.Score,
                       SampleSize = source.Value.SampleSize,
                       Satisfied = source.Value.Satisfied,
                       Tolerating = source.Value.Tolerating,
                       Frustrating = source.Value.Frustrating,
                       Tags = source.Tags.ToDictionary()
                   };
        }
    }
}