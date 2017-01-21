// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Internal.Builders
{
    public class DefaultTimerBuilder : IBuildTimerMetrics
    {
        private readonly DefaultSamplingReservoirProvider _defaultSamplingReservoirProvider;

        public DefaultTimerBuilder(DefaultSamplingReservoirProvider defaultSamplingReservoirProvider)
        {
            _defaultSamplingReservoirProvider = defaultSamplingReservoirProvider;
        }

        /// <inheritdoc />
        public ITimerMetric Build(IHistogramMetric histogram, IClock clock)
        {
            return new TimerMetric(histogram, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Build(Lazy<IReservoir> reservoir, IClock clock)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance();
            }

            return new TimerMetric(reservoir, clock);
        }
    }
}