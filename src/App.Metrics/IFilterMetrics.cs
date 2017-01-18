// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Data;

namespace App.Metrics
{
    public interface IFilterMetrics
    {
        bool ReportEnvironment { get; }

        bool ReportHealthChecks { get; }

        bool IsMatch(string context);

        bool IsMatch(GaugeValueSource gauge);

        bool IsMatch(CounterValueSource counter);

        bool IsMatch(MeterValueSource meter);

        bool IsMatch(HistogramValueSource histogram);

        bool IsMatch(TimerValueSource timer);

        bool IsMatch(ApdexValueSource apdex);
    }
}