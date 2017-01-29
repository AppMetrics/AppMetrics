// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Abstractions.Filtering
{
    public interface IFilterMetrics
    {
        bool ReportEnvironment { get; }

        bool ReportHealthChecks { get; }

        bool IsMatch(string context);

        bool IsGaugeMatch(GaugeValueSource gauge);

        bool IsCounterMatch(CounterValueSource counter);

        bool IsMeterMatch(MeterValueSource meter);

        bool IsHistogramMatch(HistogramValueSource histogram);

        bool IsTimerMatch(TimerValueSource timer);

        bool IsApdexMatch(ApdexValueSource apdex);
    }
}