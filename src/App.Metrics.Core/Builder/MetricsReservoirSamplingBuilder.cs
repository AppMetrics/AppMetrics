// <copyright file="MetricsReservoirSamplingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Scheduling;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IReservoir" /> sampling using an <see cref="IMetricsBuilder" />.
    /// </summary>
    public class MetricsReservoirSamplingBuilder : IMetricsReservoirSamplingBuilder
    {
        private readonly Action<DefaultSamplingReservoirProvider> _defaultReservoir;
        private readonly IMetricsBuilder _metricsBuilder;

        internal MetricsReservoirSamplingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<DefaultSamplingReservoirProvider> defaultReservoir)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _defaultReservoir = defaultReservoir ?? throw new ArgumentNullException(nameof(defaultReservoir));
        }

        /// <inheritdoc />
        public IMetricsBuilder AlgorithmR(int sampleSize)
        {
            Reservoir(() => new DefaultAlgorithmRReservoir(sampleSize));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder AlgorithmR()
        {
            Reservoir<DefaultAlgorithmRReservoir>();

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock, IScheduler rescaleScheduler)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock, rescaleScheduler));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying()
        {
            Reservoir<DefaultForwardDecayingReservoir>();

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Reservoir(Func<IReservoir> reservoirBuilder)
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(reservoirBuilder));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Reservoir<TReservoir>()
            where TReservoir : class, IReservoir, new()
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(() => new TReservoir()));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder SlidingWindow(int sampleSize)
        {
            Reservoir(() => new DefaultSlidingWindowReservoir(sampleSize));

            return _metricsBuilder;
        }

        /// <inheritdoc />
        public IMetricsBuilder SlidingWindow()
        {
            Reservoir<DefaultSlidingWindowReservoir>();

            return _metricsBuilder;
        }
    }
}