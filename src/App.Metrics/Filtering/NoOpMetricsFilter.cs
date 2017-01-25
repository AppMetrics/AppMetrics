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

namespace App.Metrics.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpMetricsFilter : IFilterMetrics
    {
        public bool ReportEnvironment => true;

        public bool ReportHealthChecks => true;

        public bool IsMatch(string context) { return true; }

        public bool IsMatch(GaugeValueSource gauge) { return true; }

        public bool IsMatch(CounterValueSource counter) { return true; }

        public bool IsMatch(MeterValueSource meter) { return true; }

        public bool IsMatch(HistogramValueSource histogram) { return true; }

        public bool IsMatch(TimerValueSource timer) { return true; }

        public bool IsMatch(ApdexValueSource apdex) { return true; }
    }
}