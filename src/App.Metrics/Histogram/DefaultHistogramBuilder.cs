// <copyright file="DefaultHistogramBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Histogram.Abstractions;
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
        public IHistogramMetric Build(Lazy<IReservoir> reservoir)
        {
            if (reservoir == null)
            {
                reservoir = _defaultSamplingReservoirProvider.Instance();
            }

            return new DefaultHistogramMetric(reservoir);
        }
    }
}