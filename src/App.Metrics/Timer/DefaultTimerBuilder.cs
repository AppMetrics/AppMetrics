// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer.Abstractions;

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