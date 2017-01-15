// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultMeterManager : IMeasureMeterMetrics
    {
        private readonly IAdvancedMetrics _advanced;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMeterManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="advanced">The advanced metrics manager.</param>
        public DefaultMeterManager(IAdvancedMetrics advanced, IMetricsRegistry registry)
        {
            _advanced = advanced;
            _registry = registry;
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options)
        {
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark();
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount)
        {
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark(amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, string item)
        {
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark(item);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, Action<MetricItem> itemSetup)
        {
            var item = new MetricItem();
            itemSetup(item);
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark(item, amount);
        }

        /// <inheritdoc />
        public void Mark(MeterOptions options, long amount, string item)
        {
            _registry.Meter(options, () => _advanced.BuildMeter(options)).Mark(item, amount);
        }
    }
}