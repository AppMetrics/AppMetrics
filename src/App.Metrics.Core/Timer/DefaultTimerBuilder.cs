// <copyright file="DefaultTimerBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;

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
        public ITimerMetric Build(Func<IReservoir> setupReservoir, IClock clock)
        {
            if (setupReservoir == null)
            {
                setupReservoir = _defaultSamplingReservoirProvider.Instance;
            }

            var reservoir = setupReservoir() ?? _defaultSamplingReservoirProvider.Instance();

            return new DefaultTimerMetric(reservoir, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Build(IHistogramMetric histogram, IMeterMetric meter, IClock clock)
        {
            return new DefaultTimerMetric(histogram, meter, clock);
        }

        /// <inheritdoc />
        public ITimerMetric Build(Func<IReservoir> setupReservoir, IMeterMetric meter, IClock clock)
        {
            if (setupReservoir == null)
            {
                setupReservoir = _defaultSamplingReservoirProvider.Instance;
            }

            var reservoir = setupReservoir() ?? _defaultSamplingReservoirProvider.Instance();

            return new DefaultTimerMetric(reservoir, meter, clock);
        }
    }
}