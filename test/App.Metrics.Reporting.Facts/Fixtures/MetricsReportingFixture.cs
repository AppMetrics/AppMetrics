// <copyright file="MetricsReportingFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Filtering;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Core.Internal;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.Reporting.Facts.TestHelpers;
using App.Metrics.Reporting.Internal;
using App.Metrics.Timer;
using Microsoft.Extensions.Logging;
using Moq;

namespace App.Metrics.Reporting.Facts.Fixtures
{
    public class MetricsReportingFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsReportingFixture()
        {
            var options = new AppMetricsOptions();
            var clock = new TestClock();

            IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

            DefaultMetrics defaultMetrics;
            ReportGenerator = new DefaultReportGenerator(new LoggerFactory());

            Metrics = () =>
            {
                var registry = new DefaultMetricsRegistry(_loggerFactory, options, clock, NewContextRegistry);
                var metricBuilderFactory = new DefaultMetricsBuilderFactory();
                var filter = new DefaultMetricsFilter();
                var dataManager = new DefaultMetricValuesProvider(
                    filter,
                    registry);

                var metricsManagerFactory = new DefaultMeasureMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManagerAdvancedFactory = new DefaultMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManager = new DefaultMetricsManager(registry, _loggerFactory.CreateLogger<DefaultMetricsManager>());

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

        internal DefaultReportGenerator ReportGenerator { get; }

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