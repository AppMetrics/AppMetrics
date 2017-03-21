// <copyright file="ApdexValueSourceExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
            return valueService.GetForContext(context).ApdexScores.ValueFor(metricName);
        }

        public static ApdexValue GetValueOrDefault(this IApdex metric)
        {
            var implementation = metric as IApdexMetric;
            return implementation != null ? implementation.Value : EmptyApdex;
        }
    }
}