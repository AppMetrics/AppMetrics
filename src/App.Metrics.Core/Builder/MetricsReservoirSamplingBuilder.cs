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

        internal MetricsReservoirSamplingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<DefaultSamplingReservoirProvider> defaultReservoir)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _defaultReservoir = defaultReservoir ?? throw new ArgumentNullException(nameof(defaultReservoir));
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder AlgorithmR(int sampleSize)
        {
            Reservoir(() => new DefaultAlgorithmRReservoir(sampleSize));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder AlgorithmR()
        {
            Reservoir<DefaultAlgorithmRReservoir>();

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock, IReservoirRescaleScheduler rescaleScheduler)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock, rescaleScheduler));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying()
        {
            Reservoir<DefaultForwardDecayingReservoir>();

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Reservoir(Func<IReservoir> reservoirBuilder)
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(reservoirBuilder));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Reservoir<TReservoir>()
            where TReservoir : class, IReservoir, new()
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(() => new TReservoir()));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder SlidingWindow(int sampleSize)
        {
            Reservoir(() => new DefaultSlidingWindowReservoir(sampleSize));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder SlidingWindow()
        {
            Reservoir<DefaultSlidingWindowReservoir>();

            return Builder;
        }
    }
}