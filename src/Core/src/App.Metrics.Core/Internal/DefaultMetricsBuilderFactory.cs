// <copyright file="DefaultMetricsBuilderFactory.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricsBuilderFactory : IBuildMetrics
    {
        // Internal for testing
#pragma warning disable SA1401 // Fields must be private
        internal readonly DefaultSamplingReservoirProvider DefaultSamplingReservoir;
#pragma warning restore SA1401 // Fields must be private

        public DefaultMetricsBuilderFactory(DefaultSamplingReservoirProvider defaultSamplingReservoir)
        {
            DefaultSamplingReservoir = defaultSamplingReservoir;
            Apdex = new DefaultApdexBuilder(defaultSamplingReservoir);
            Counter = new DefaultCounterBuilder();
            Gauge = new DefaultGaugeBuilder();
            Histogram = new DefaultHistogramBuilder(defaultSamplingReservoir);
            BucketHistogram = new DefaultBucketHistogramBuilder();
            Meter = new DefaultMeterBuilder();
            Timer = new DefaultTimerBuilder(defaultSamplingReservoir);
            BucketTimer = new DefaultBucketTimerBuilder();
        }

        /// <inheritdoc />
        public IBuildApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IBuildCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IBuildGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IBuildHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IBuildBucketHistogramMetrics BucketHistogram { get; }

        /// <inheritdoc />
        public IBuildMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IBuildTimerMetrics Timer { get; }

        /// <inheritdoc />
        public IBuildBucketTimerMetrics BucketTimer { get; }
    }
}