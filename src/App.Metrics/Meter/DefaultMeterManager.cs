// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Options;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Meter
{
    internal sealed class DefaultMeterManager : IMeasureMeterMetrics
    {
        private readonly IClock _clock;
        private readonly IBuildMeterMetrics _meterBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeterManager" /> class.
        /// </summary>
        /// <param name="clock">The clock.</param>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="meterBuilder">The meter builder.</param>
        public DefaultMeterManager(IBuildMeterMetrics meterBuilder, IMetricsRegistry registry, IClock clock)
        {
            _clock = clock;
            _registry = registry;
            _meterBuilder = meterBuilder;
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark();
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, string item)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricSetItem itemSetup)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(itemSetup);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, MetricSetItem itemSetup)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(itemSetup, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, string item)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(item, amount);
        }
    }
}