// <copyright file="DefaultMeterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Meter
{
    public class DefaultMeterBuilder : IBuildMeterMetrics
    {
        /// <inheritdoc />
        public IMeterMetric Build(IClock clock) { return new DefaultMeterMetric(clock); }
    }
}