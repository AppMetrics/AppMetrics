// <copyright file="DefaultHistogramBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Histogram
{
    public class DefaultHistogramBuilder : IBuildHistogramMetrics
    {
        private readonly DefaultSamplingReservoirProvider _defaultSamplingReservoirProvider;

        public DefaultHistogramBuilder(DefaultSamplingReservoirProvider defaultSamplingReservoirProvider)
        {
            _defaultSamplingReservoirProvider = defaultSamplingReservoirProvider;
        }

        /// <inheritdoc />
        public IHistogramMetric Build(Func<IReservoir> setupReservoir)
        {
            if (setupReservoir == null)
            {
                setupReservoir = _defaultSamplingReservoirProvider.Instance;
            }

            var reservoir = setupReservoir() ?? _defaultSamplingReservoirProvider.Instance();

            return new DefaultHistogramMetric(reservoir);
        }
    }
}