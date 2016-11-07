// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using App.Metrics.Data;
using App.Metrics.Utils;

namespace App.Metrics
{
    public interface IMetricsFilter : IHideObjectMembers
    {
        bool IsMatch(string group);

        bool IsMatch(GaugeValueSource gauge);

        bool IsMatch(CounterValueSource counter);

        bool IsMatch(MeterValueSource meter);

        bool IsMatch(HistogramValueSource histogram);

        bool IsMatch(TimerValueSource timer);

        bool ReportEnvironment { get; }

        bool ReportHealthChecks { get; }
    }
}