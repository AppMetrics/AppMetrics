// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Data;

namespace App.Metrics.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpMetricsFilter : IMetricsFilter
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