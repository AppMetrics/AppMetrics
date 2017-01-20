// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Interfaces
{
    public interface IProvideMetrics
    {
        IProvideApdexMetrics Apdex { get; }

        IProvideCounterMetrics Counter { get; }

        IProvideGaugeMetrics Gauge { get; }

        IProvideHistogramMetrics Histogram { get; }

        IProviderMeterMetrics Meter { get; }

        IProvideTimerMetrics Timer { get; }
    }
}