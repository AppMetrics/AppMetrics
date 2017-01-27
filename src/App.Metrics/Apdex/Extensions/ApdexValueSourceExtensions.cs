// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Apdex
    // ReSharper restore CheckNamespace
{
    public static class ApdexValueSourceExtensions
    {
        private static readonly ApdexValue EmptyApdex = new ApdexValue(0.0, 0, 0, 0, 0);

        public static ApdexValue GetApdexValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).ApdexScores.ValueFor(context, metricName);
        }

        public static ApdexValue Value(this IApdex metric)
        {
            var implementation = metric as IApdexMetric;
            return implementation != null ? implementation.Value : EmptyApdex;
        }

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