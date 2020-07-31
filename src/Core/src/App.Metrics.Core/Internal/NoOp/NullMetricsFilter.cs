// <copyright file="NullMetricsFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Filters;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public sealed class NullMetricsFilter : IFilterMetrics
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
        public bool IsBucketHistogramMatch(BucketHistogramValueSource histogram) { return true; }

        /// <inheritdoc />
        public bool IsContextMatch(string context) { return true; }

        /// <inheritdoc />
        public bool IsMeterMatch(MeterValueSource meter) { return true; }

        /// <inheritdoc />
        public bool IsTimerMatch(TimerValueSource timer) { return true; }

        /// <inheritdoc />
        public bool IsBucketTimerMatch(BucketTimerValueSource timer) { return true; }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(Predicate<string> condition) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereContext(string context) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereName(string name) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereName(Predicate<string> condition) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereNameStartsWith(string name) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereTaggedWithKey(params string[] tagKeys) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereTaggedWithKeyValue(TagKeyValueFilter tags) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WhereType(params MetricType[] types) { return this; }
    }
}