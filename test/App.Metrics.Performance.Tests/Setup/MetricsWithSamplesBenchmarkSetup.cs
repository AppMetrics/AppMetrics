using System;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Performance.Tests.Setup
{
    public static class MetricsFactory
    {
        public static IMetrics Instance()
        {
            var loggerFactory = new LoggerFactory();
            var metricsLogger = loggerFactory.CreateLogger<DefaultAdvancedMetrics>();
            var healthFactoryLogger = loggerFactory.CreateLogger<HealthCheckFactory>();
            var clock = new TestClock();
            var options = new AppMetricsOptions { DefaultSamplingType = SamplingType.LongTerm };
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(loggerFactory, options, clock, new EnvironmentInfoBuilder(loggerFactory), newContextRegistry);
            var healthCheckFactory = new HealthCheckFactory(healthFactoryLogger);
            var advancedContext = new DefaultAdvancedMetrics(metricsLogger, options, clock, new DefaultMetricsFilter(), registry, healthCheckFactory);

            var metrics = new DefaultMetrics(options, registry, advancedContext);

            RecordSomeMetrics(metrics);

            return metrics;
        }

        private static void RecordSomeMetrics(IMetrics metrics)
        {
            var counterOptions = new CounterOptions
            {
                Name = "test_counter",
                MeasurementUnit = Unit.Requests,
                Tags = new MetricTags().With("tag1", "value")
            };

            var meterOptions = new MeterOptions
            {
                Name = "test_meter",
                MeasurementUnit = Unit.None,
                Tags = new MetricTags().With("tag2", "value")
            };

            var timerOptions = new TimerOptions
            {
                Name = "test_timer",
                MeasurementUnit = Unit.Requests
            };

            var histogramOptions = new HistogramOptions
            {
                Name = "test_histogram",
                MeasurementUnit = Unit.Requests
            };

            var gaugeOptions = new GaugeOptions
            {
                Name = "test_gauge"
            };

            metrics.Increment(counterOptions);
            metrics.Mark(meterOptions);
            metrics.Time(timerOptions, () => metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, 10));
            metrics.Update(histogramOptions, 5);
            metrics.Gauge(gaugeOptions, () => 8);
        }
    }
}