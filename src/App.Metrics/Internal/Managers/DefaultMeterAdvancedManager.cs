// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Managers
{
    public class DefaultMeterAdvancedManager : IMeasureMeterMetricsAdvanced
    {
        private readonly IClock _clock;
        private readonly IBuildMeterMetrics _meterBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeterAdvancedManager" /> class.
        /// </summary>
        /// <param name="meterBuilder">The meter builder.</param>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMeterAdvancedManager(IBuildMeterMetrics meterBuilder, IMetricsRegistry registry, IClock clock)
        {
            _registry = registry;
            _clock = clock;
            _meterBuilder = meterBuilder;
        }

        /// <inheritdoc />
        public IMeter With(MeterOptions options) { return With(options, () => _meterBuilder.Instance(_clock)); }

        /// <inheritdoc />
        public IMeter With<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric { return _registry.Meter(options, builder); }
    }
}