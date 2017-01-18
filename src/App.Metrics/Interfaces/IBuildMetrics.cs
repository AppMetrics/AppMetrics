// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Interfaces
{
    public interface IBuildMetrics
    {
        IBuildApdexMetrics Apdex { get; }

        IBuildCounterMetrics Counter { get; }

        IBuildGaugeMetrics Gauge { get; }

        IBuildHistogramMetrics Histogram { get; }

        IBuildMeterMetrics Meter { get; }

        IBuildTimerMetrics Timer { get; }
    }
}