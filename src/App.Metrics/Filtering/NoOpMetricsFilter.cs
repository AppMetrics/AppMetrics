// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Apdex;
using App.Metrics.Core.Internal;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics.Internal
    // ReSharper restore CheckNamespace
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpMetricsFilter : IFilterMetrics
    {
        /// <inheritdoc />
        public bool ReportEnvironment => true;

        /// <inheritdoc />
        public bool ReportHealthChecks => true;

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

        /// <inheritdoc />
        public IFilterMetrics WithEnvironmentInfo(bool report) { return this; }

        /// <inheritdoc />
        public IFilterMetrics WithHealthChecks(bool report) { return this; }
    }
}