// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;

// ReSharper disable CheckNamespace
namespace App.Metrics.Apdex
    // ReSharper restore CheckNamespace
{
    public static class ApdexValueSourceExtensions
    {
        public static IEnumerable<ApdexMetric> ToMetric(this IEnumerable<ApdexValueSource> source) { return source.Select(ToMetric); }

        public static ApdexMetric ToMetric(this ApdexValueSource source)
        {
            return new ApdexMetric
                   {
                       Name = source.Name,
                       Score = source.Value.Score,
                       SampleSize = source.Value.SampleSize,
                       Satisfied = source.Value.Satisfied,
                       Tolerating = source.Value.Tolerating,
                       Frustrating = source.Value.Frustrating,
                       Tags = source.Tags
                   };
        }

        public static ApdexValueSource ToMetricValueSource(this ApdexMetric source)
        {
            var counterValue = new ApdexValue(source.Score, source.Satisfied, source.Tolerating, source.Frustrating, source.SampleSize);
            return new ApdexValueSource(source.Name, ConstantValue.Provider(counterValue), source.Tags);
        }

        public static IEnumerable<ApdexValueSource> ToMetricValueSource(this IEnumerable<ApdexMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}