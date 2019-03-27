// <copyright file="IMetricsReservoirSamplingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;
using App.Metrics.Scheduling;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricsReservoirSamplingBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultAlgorithmRReservoir" reservoir for <see cref="MetricType" />s which require
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
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder AlgorithmR(int sampleSize);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultAlgorithmRReservoir" reservoir for <see cref="MetricType" />s which require
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
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder AlgorithmR();

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(int sampleSize, double alpha);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        /// <param name="clock">The <see cref="IClock" /> used for timing.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight, IClock clock);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        /// <param name="minimumSampleWeight">
        ///     Minimum weight required for a sample to be retained during reservoir rescaling. Samples with weights less than this value will be discarded.
        ///     This behavior is useful if there are longer periods of very low or no activity. Default value is zero, which preserves all samples during rescaling.
        /// </param>
        /// <param name="clock">The <see cref="IClock" /> used for timing.</param>
        /// <param name="rescaleScheduler">The <see cref="IReservoirRescaleScheduler" /> used to rescale the reservoir.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(int sampleSize, double alpha, double minimumSampleWeight, IClock clock, IReservoirRescaleScheduler rescaleScheduler);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        /// <param name="alpha">
        ///     The alpha value, e.g 0.015 will heavily biases the reservoir to the past 5 mins of measurements. The higher the
        ///     value the more biased the reservoir will be towards newer values.
        /// </param>
        /// <param name="rescalePeriod">The interval at which the reservoir will be rescaled.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(double alpha, TimeSpan rescalePeriod);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        /// <param name="rescalePeriod">The interval at which the reservoir will be rescaled.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying(TimeSpan rescalePeriod);

        /// <summary>
        ///     <para>
        ///         Uses the "DefaultForwardDecayingReservoir" reservoir for <see cref="MetricType" />s which
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
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder ForwardDecaying();

        /// <summary>
        ///     Uses the specifed <see cref="IReservoir" /> for <see cref="MetricType" />s which require sampling.
        /// </summary>
        /// <param name="reservoirBuilder">
        ///     An <see cref="IReservoir" /> function used to sample metrics.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Reservoir(Func<IReservoir> reservoirBuilder);

        /// <summary>
        ///     Uses the specifed <see cref="IReservoir" /> for <see cref="MetricType" />s which require sampling.
        /// </summary>
        /// <typeparam name="TReservoir">An <see cref="IReservoir" /> type used to sample metrics.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder Reservoir<TReservoir>()
            where TReservoir : class, IReservoir, new();

        /// <summary>
        ///     Uses "DefaultSlidingWindowReservoir" reservoir sample for <see cref="MetricType" />s which
        ///     require sampling. A Reservoir implementation backed by a sliding window that stores only the measurements made in
        ///     the last N seconds (or other time unit).
        /// </summary>
        /// <param name="sampleSize">The number of samples to keep in the sampling reservoir.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder SlidingWindow(int sampleSize);

        /// <summary>
        ///     Uses "DefaultSlidingWindowReservoir" reservoir sample for <see cref="MetricType" />s which
        ///     require sampling. A Reservoir implementation backed by a sliding window that stores only the measurements made in
        ///     the last N seconds (or other time unit).
        /// </summary>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        IMetricsBuilder SlidingWindow();
    }
}