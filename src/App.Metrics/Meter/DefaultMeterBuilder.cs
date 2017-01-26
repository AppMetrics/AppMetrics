// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Meter.Abstractions;

namespace App.Metrics.Meter
{
    public class DefaultMeterBuilder : IBuildMeterMetrics
    {
        /// <inheritdoc />
        public IMeterMetric Build(IClock clock) { return new DefaultMeterMetric(clock); }
    }
}