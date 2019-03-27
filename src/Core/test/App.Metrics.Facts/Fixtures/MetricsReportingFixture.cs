// <copyright file="MetricsReportingFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Counter;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.FactsCommon;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Internal;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsReportingFixture : IDisposable
    {
        public MetricsReportingFixture()
        {
            var options = new MetricsOptions();
            var clock = new TestClock();

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            DefaultMetrics defaultMetrics;

            Metrics = () =>
            {
                var registry = new DefaultMetricsRegistry(options.DefaultContextLabel, clock, NewContextRegistry);
                var metricBuilderFactory = new DefaultMetricsBuilderFactory(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));
                var filter = new MetricsFilter();
                var dataManager = new DefaultMetricValuesProvider(
                    filter,
                    registry);

                var metricsManagerFactory = new DefaultMeasureMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManagerAdvancedFactory = new DefaultMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManager = new DefaultMetricsManager(registry);

                defaultMetrics = new DefaultMetrics(
                    clock,
                    filter,
                    metricsManagerFactory,
                    metricBuilderFactory,
                    metricsManagerAdvancedFactory,
                    dataManager,
                    metricsManager);

                RecordSomeMetrics(defaultMetrics);

                return defaultMetrics;
            };
        }

        public Func<IMetrics> Metrics { get; }

        public void Dispose() { Dispose(true); }

        protected virtual void Dispose(bool disposing) { }

        private void RecordSomeMetrics(IMetrics metrics)
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test_counter",
                                     MeasurementUnit = Unit.Requests,
                                     Tags = new MetricTags("tag1", "value")
                                 };

            var meterOptions = new MeterOptions
                               {
                                   Name = "test_meter",
                                   MeasurementUnit = Unit.None,
                                   Tags = new MetricTags("tag2", "value")
                               };

            var timerOptions = new TimerOptions
                               {
                                   Name = "test_timer",
                                   Reservoir = () => new CustomReservoir(),
                                   MeasurementUnit = Unit.Requests
                               };

            var histogramOptions = new HistogramOptions
                                   {
                                       Name = "test_histogram",
                                       Reservoir = () => new CustomReservoir(),
                                       MeasurementUnit = Unit.Requests
                                   };

            var gaugeOptions = new GaugeOptions
                               {
                                   Name = "test_gauge"
                               };

            metrics.Measure.Counter.Increment(counterOptions);
            metrics.Measure.Meter.Mark(meterOptions);
            metrics.Measure.Timer.Time(timerOptions, () => metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            metrics.Measure.Histogram.Update(histogramOptions, 5);
            metrics.Measure.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}