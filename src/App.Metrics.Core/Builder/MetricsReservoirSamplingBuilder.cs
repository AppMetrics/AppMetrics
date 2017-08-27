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
    public class MetricsReservoirSamplingBuilder
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

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultAlgorithmRReservoir" /> reservoir for <see cref="MetricType" />s which require
        ///         sampling.
        ///     </para>
        ///     <para>
        ///         A histogram with a uniform reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see>
        ///         which are valid for the entirely of the histogram’s lifetime.
        ///     </para>
        ///     <para>
        ///         This sampling reservoir can be used when you are interested in long-term measurements, it does not offer a
        ///         sence of recency.
        ///     </para>
        ///     <para>
        ///         All samples are equally likely to be evicted when the reservoir is at full capacity.
        ///     </para>
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder AlgorithmR(int sampleSize)
        {
            Reservoir(() => new DefaultAlgorithmRReservoir(sampleSize));

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultAlgorithmRReservoir" /> reservoir for <see cref="MetricType" />s which require
        ///         sampling.
        ///     </para>
        ///     <para>
        ///         A histogram with a uniform reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see>
        ///         which are valid for the entirely of the histogram’s lifetime.
        ///     </para>
        ///     <para>
        ///         This sampling reservoir can be used when you are interested in long-term measurements, it does not offer a
        ///         sence of recency.
        ///     </para>
        ///     <para>
        ///         All samples are equally likely to be evicted when the reservoir is at full capacity.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder AlgorithmR()
        {
            Reservoir<DefaultAlgorithmRReservoir>();

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultForwardDecayingReservoir" /> reservoir for <see cref="MetricType" />s which
        ///         require sampling.
        ///         A histogram with an exponentially decaying reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the
        ///         last five minutes of data.
        ///     </para>
        ///     <para>
        ///         The reservoir is produced by using a
        ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying reservoir</see> with an
        ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
        ///     </para>
        ///     <para>
        ///         This sampling reservoir can be used when you are interested in recent changes to the distribution of data
        ///         rather than a median on the lifetime of the histgram.
        ///     </para>
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha));

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultForwardDecayingReservoir" /> reservoir for <see cref="MetricType" />s which
        ///         require sampling.
        ///         A histogram with an exponentially decaying reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the
        ///         last five minutes of data.
        ///     </para>
        ///     <para>
        ///         The reservoir is produced by using a
        ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying reservoir</see> with an
        ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
        ///     </para>
        ///     <para>
        ///         This sampling reservoir can be used when you are interested in recent changes to the distribution of data
        ///         rather than a median on the lifetime of the histgram.
        ///     </para>
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="clock">The <see cref="IClock" /> used for timing.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock));

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultForwardDecayingReservoir" /> reservoir for <see cref="MetricType" />s which
        ///         require sampling.
        ///         A histogram with an exponentially decaying reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the
        ///         last five minutes of data.
        ///     </para>
        ///     <para>
        ///         The reservoir is produced by using a
        ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying reservoir</see> with an
        ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
        ///     </para>
        ///     <para>
        ///         This sampling reservoir can be used when you are interested in recent changes to the distribution of data
        ///         rather than a median on the lifetime of the histgram.
        ///     </para>
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="clock">The <see cref="IClock" /> used for timing.</param>
        /// <param name="rescaleScheduler">The <see cref="IScheduler" /> used to rescale the reservoir.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, IClock clock, IScheduler rescaleScheduler)
        {
            Reservoir(() => new DefaultForwardDecayingReservoir(sampleSize, alpha, clock, rescaleScheduler));

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the <see cref="DefaultForwardDecayingReservoir" /> reservoir for <see cref="MetricType" />s which
        ///         require sampling.
        ///         A histogram with an exponentially decaying reservoir produces
        ///         <see href="https://en.wikipedia.org/wiki/Quantile">quantiles</see> which are representative of (roughly) the
        ///         last five minutes of data.
        ///     </para>
        ///     <para>
        ///         The reservoir is produced by using a
        ///         <see href="http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf">forward-decaying reservoir</see> with an
        ///         exponential weighty towards recent data unlike a Uniform Reservoir which does not provide a sense of recency.
        ///     </para>
        ///     <para>
        ///         This samling reservoir can be used when you are interested in recent changes to the distribution of data
        ///         rather than a median on the lifetime of the histgram.
        ///     </para>
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder ForwardDecaying()
        {
            Reservoir<DefaultForwardDecayingReservoir>();

            return _metricsBuilder;
        }

        /// <summary>
        ///     Uses the specifed <see cref="IReservoir" /> for <see cref="MetricType" />s which require sampling.
        /// </summary>
        /// <param name="reservoirBuilder">
        ///     An <see cref="IReservoir" /> function used to sample metrics.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Reservoir(Func<IReservoir> reservoirBuilder)
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(reservoirBuilder));

            return _metricsBuilder;
        }

        /// <summary>
        ///     Uses the specifed <see cref="IReservoir" /> for <see cref="MetricType" />s which require sampling.
        /// </summary>
        /// <typeparam name="TReservoir">An <see cref="IReservoir" /> type used to sample metrics.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Reservoir<TReservoir>()
            where TReservoir : class, IReservoir, new()
        {
            _defaultReservoir(new DefaultSamplingReservoirProvider(() => new TReservoir()));

            return _metricsBuilder;
        }

        /// <summary>
        ///     Uses <see cref="DefaultSlidingWindowReservoir" /> reservoir sample for <see cref="MetricType" />s which
        ///     require sampling. A Reservoir implementation backed by a sliding window that stores only the measurements made in
        ///     the last N seconds (or other time unit).
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder SlidingWindow(int sampleSize)
        {
            Reservoir(() => new DefaultSlidingWindowReservoir(sampleSize));

            return _metricsBuilder;
        }

        /// <summary>
        ///     Uses <see cref="DefaultSlidingWindowReservoir" /> reservoir sample for <see cref="MetricType" />s which
        ///     require sampling. A Reservoir implementation backed by a sliding window that stores only the measurements made in
        ///     the last N seconds (or other time unit).
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder SlidingWindow()
        {
            Reservoir<DefaultSlidingWindowReservoir>();

            return _metricsBuilder;
        }
    }
}