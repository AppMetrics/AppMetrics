// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Apdex;
using App.Metrics.Core.Internal;
using App.Metrics.Counter;
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
        public bool ReportEnvironment => true;

        public bool ReportHealthChecks => true;

        public bool IsApdexMatch(ApdexValueSource apdex) { return true; }

        public bool IsCounterMatch(CounterValueSource counter) { return true; }

        public bool IsGaugeMatch(GaugeValueSource gauge) { return true; }

        public bool IsHistogramMatch(HistogramValueSource histogram) { return true; }

        public bool IsMatch(string context) { return true; }

        public bool IsMeterMatch(MeterValueSource meter) { return true; }

        public bool IsTimerMatch(TimerValueSource timer) { return true; }
    }
}