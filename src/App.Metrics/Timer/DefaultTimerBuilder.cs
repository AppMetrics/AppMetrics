// <copyright file="DefaultTimerBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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

        /// <inheritdoc />
        public ITimerMetric Build(IHistogramMetric histogram, IMeterMetric meter, IClock clock)
        {
            return new DefaultTimerMetric(histogram, meter, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Build(Lazy<IReservoir> reservoir, IMeterMetric meter, IClock clock)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance();
            }

            return new DefaultTimerMetric(reservoir, meter, clock);
        }
    }
}