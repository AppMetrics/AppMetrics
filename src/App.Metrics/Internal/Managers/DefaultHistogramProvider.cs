// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Managers
{
    public class DefaultHistogramProvider : IProvideHistogramMetrics
    {
        private readonly IBuildHistogramMetrics _histogramBuilder;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHistogramProvider" /> class.
        /// </summary>
        /// <param name="histogramBuilder">The histogram builder.</param>
        /// <param name="registry">The registry.</param>
        public DefaultHistogramProvider(IBuildHistogramMetrics histogramBuilder, IMetricsRegistry registry)
        {
            _registry = registry;
            _histogramBuilder = histogramBuilder;
        }

        /// <inheritdoc />
        public IHistogram Instance(HistogramOptions options)
        {
            if (options.WithReservoir != null)
            {
                return Instance(options, () => _histogramBuilder.Build(options.WithReservoir()));
            }

            return Instance(options, () => _histogramBuilder.Build(options.SamplingType, options.SampleSize, options.ExponentialDecayFactor));
        }

        /// <inheritdoc />
        public IHistogram Instance<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric { return _registry.Histogram(options, builder); }
    }
}