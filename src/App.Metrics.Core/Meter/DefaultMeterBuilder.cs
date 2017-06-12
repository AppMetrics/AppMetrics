// <copyright file="DefaultMeterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Meter;

namespace App.Metrics.Core.Meter
{
    public class DefaultMeterBuilder : IBuildMeterMetrics
    {
        /// <inheritdoc />
        public IMeterMetric Build(IClock clock) { return new DefaultMeterMetric(clock); }
    }
}