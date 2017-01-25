// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Timer
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
            return new DefaultTimerMetric(histogram, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Build(Lazy<IReservoir> reservoir, IClock clock)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance();
            }

            return new DefaultTimerMetric(reservoir, clock);
        }
    }
}