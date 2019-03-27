// <copyright file="MetricsReservoirSamplingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Infrastructure;
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
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, minimumSampleWeight));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight, IClock clock)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, minimumSampleWeight, clock));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight, IClock clock, IReservoirRescaleScheduler rescaleScheduler)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, minimumSampleWeight, clock, rescaleScheduler));

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(TimeSpan rescalePeriod)
        {
            ForwardDecaying(AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor, rescalePeriod);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder ForwardDecaying(double alpha, TimeSpan rescalePeriod)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                new StopwatchClock(),
                new DefaultReservoirRescaleScheduler(rescalePeriod)));

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