// <copyright file="NoOpMetricsFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Filters;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core.Internal
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal sealed class NoOpMetricsFilter : IFilterMetrics
    {
        /// <inheritdoc />
        public bool IsApdexMatch(ApdexValueSource apdex) { return true; }

        /// <inheritdoc />
        public bool IsCounterMatch(CounterValueSource counter) { return true; }

        /// <inheritdoc />
        public bool IsGaugeMatch(GaugeValueSource gauge) { return true; }

        /// <inheritdoc />
        public bool IsHistogramMatch(HistogramValueSource histogram) { return true; }

        /// <inheritdoc />
        public bool IsContextMatch(string context) { return true; }

        /// <inheritdoc />
        public bool IsMeterMatch(MeterValueSource meter) { return true; }

        /// <inheritdoc />
        public bool IsTimerMatch(TimerValueSource timer) { return true; }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(Predicate<string> condition) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(string context) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereMetricName(Predicate<string> condition) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereMetricNameStartsWith(string name) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereMetricTaggedWithKey(params string[] tagKeys) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereMetricTaggedWithKeyValue(TagKeyValueFilter tags) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereType(params MetricType[] types) { return this; }
    }
}