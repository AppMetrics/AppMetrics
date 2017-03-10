// <copyright file="DefaultMeterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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