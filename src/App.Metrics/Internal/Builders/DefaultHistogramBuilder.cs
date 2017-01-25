// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Internal.Builders
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

            return new HistogramMetric(reservoir);
        }
    }
}