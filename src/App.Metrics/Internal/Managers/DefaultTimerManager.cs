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
    internal class DefaultTimerManager : IMeasureTimerMetrics
    {
        private readonly IAdvancedMetrics _advanced;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTimerManager" /> class.
        /// </summary>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="advanced">The advanced metrics manager.</param>
        public DefaultTimerManager(IAdvancedMetrics advanced, IMetricsRegistry registry)
        {
            _advanced = advanced;
            _registry = registry;
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action)
        {
            using (_registry.Timer(options, () => _advanced.BuildTimer(options)).NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Time(TimerOptions options, Action action, string userValue)
        {
            using (_registry.Timer(options, () => _advanced.BuildTimer(options)).NewContext(userValue))
            {
                action();
            }
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options)
        {
            return _registry.Timer(options, () => _advanced.BuildTimer(options)).NewContext();
        }

        /// <inheritdoc />
        public TimerContext Time(TimerOptions options, string userValue)
        {
            return _registry.Timer(options, () => _advanced.BuildTimer(options)).NewContext(userValue);
        }
    }
}