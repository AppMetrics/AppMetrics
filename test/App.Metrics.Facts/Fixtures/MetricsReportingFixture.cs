using System;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Core.Options;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsReportingFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsReportingFixture()
        {
            var metricsLogger = _loggerFactory.CreateLogger<DefaultAdvancedMetrics>();
            var healthFactoryLogger = _loggerFactory.CreateLogger<HealthCheckFactory>();
            var options = new AppMetricsOptions { DefaultSamplingType = SamplingType.LongTerm };
            var clock = new TestClock();
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(_loggerFactory, options, clock, new EnvironmentInfoProvider(), newContextRegistry);
            var healthCheckFactory = new HealthCheckFactory(healthFactoryLogger);
            var advancedContext = new DefaultAdvancedMetrics(metricsLogger, options, clock, new DefaultMetricsFilter(), registry, healthCheckFactory);
            Metrics = new DefaultMetrics(options, registry, advancedContext);

            RecordSomeMetrics();
        }

        public IMetrics Metrics { get; }

        public Func<IMetrics, Task<MetricsDataValueSource>> CurrentData =>
            async ctx => await Metrics.Advanced.Data.ReadDataAsync();

        public Func<IMetrics, IMetricsFilter, Task<MetricsDataValueSource>> CurrentDataWithFilter
            => async (ctx, filter) => await Metrics.Advanced.Data.ReadDataAsync(filter);

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            Metrics?.Advanced.Data.Reset();
        }

        private void RecordSomeMetrics()
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

            Metrics.Increment(counterOptions);
            Metrics.Mark(meterOptions);
            Metrics.Time(timerOptions, () => Metrics.Advanced.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Update(histogramOptions, 5);
            Metrics.Gauge(gaugeOptions, () => 8);
        }
    }
}