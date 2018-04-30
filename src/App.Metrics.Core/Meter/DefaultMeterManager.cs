// <copyright file="DefaultMeterManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Registry;

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
        public void Mark(MeterOptions options, MetricTags tags, long amount)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark(amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricTags tags)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark();
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, string item)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricTags tags, string item)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricSetItem setItem)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(setItem);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricTags tags, MetricSetItem setItem)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark(setItem);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricTags tags, long amount, string item)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark(item, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, MetricSetItem setItem)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(setItem, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, MetricTags tags, long amount, MetricSetItem setItem)
        {
            _registry.Meter(options, tags, () => _meterBuilder.Build(_clock)).Mark(setItem, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, string item)
        {
            _registry.Meter(options, () => _meterBuilder.Build(_clock)).Mark(item, amount);
        }
    }
}