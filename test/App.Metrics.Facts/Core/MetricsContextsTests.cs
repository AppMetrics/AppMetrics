using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Extensions;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsContextsTests
    {
        private static readonly IOptions<AppMetricsOptions> Options
           = Microsoft.Extensions.Options.Options.Create(new AppMetricsOptions());

        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        private static readonly IHealthCheckManager HealthCheckManager =
            new DefaultHealthCheckManager(LoggerFactory, new DefaultHealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options));
        private static readonly Func<string, IMetricGroupRegistry> NewMetricsGroupRegistry = name => new DefaultMetricGroupRegistry(name);

        private static readonly IMetricsRegistry Registry = new DefaultMetricsRegistry(Options.Value.GlobalContextName,
            Options.Value.DefaultSamplingType, Options.Value.SystemClock, new EnvironmentInfo(), NewMetricsGroupRegistry);

        private static readonly IMetricsDataManager MetricsDataManager =
            new DefaultMetricsDataManager(LoggerFactory, Options.Value.SystemClock, Enumerable.Empty<EnvironmentInfoEntry>(), Registry);

        private static readonly IMetricsBuilder MetricsBuilder = new DefaultMetricsBuilder(Options.Value.SystemClock, Options.Value.DefaultSamplingType);
      

        private readonly IMetricsContext _context = new DefaultMetricsContext(Options.Value.GlobalContextName,
            Options.Value.SystemClock, Registry, MetricsBuilder, HealthCheckManager, MetricsDataManager);

        public Func<IMetricsContext, MetricsData> CurrentData => ctx => _context.Advanced.MetricsDataManager.GetMetricsData();

        public Func<IMetricsContext, IMetricsFilter, MetricsData> CurrentDataWithFilter => (ctx, filter) =>
                _context.Advanced.MetricsDataManager.WithFilter(filter).GetMetricsData();

        public MetricsContextsTests()
        {
            _context.Advanced.ResetMetricsValues();
        }

        [Fact]
        public void can_create_child_context()
        {
            var counterOptions = new CounterOptions
            {
                Name = "counter",
                GroupName = "test",
                MeasurementUnit = Unit.Requests,
            };

            _context.Advanced.Counter(counterOptions);

            var counterValue = CurrentData(_context).ChildMetrics.SelectMany(c => c.Counters).Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public void can_filter_metrics_by_type()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Requests,
            };


            var meterOptions = new MeterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = "tag"
            };

            var counter = _context.Advanced.Counter(counterOptions);
            var meter = _context.Advanced.Meter(meterOptions);

            var filter = new MetricsFilter().WhereType(MetricType.Counter);

            counter.Increment();
            meter.Mark(1);

            var currentData = CurrentDataWithFilter(_context, filter);

            var counterValue = currentData.Counters.Single();
            var meterValue = currentData.Meters.FirstOrDefault();

            counterValue.Name.Should().Be("test");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(meterValue);
        }

        [Fact]
        public void can_propergate_value_tags()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = "tag"
            };

            var meterOptions = new MeterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = "tag"
            };

            var histogramOptions = new HistogramOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = "tag"
            };

            var timerOptions = new TimerOptions
            {
                Name = "test",
                MeasurementUnit = Unit.None,
                Tags = "tag"
            };

            _context.Advanced.Counter(counterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Single().Tags.Should().Equal("tag");

            _context.Advanced.Meter(meterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData().Meters.Single().Tags.Should().Equal("tag");

            _context.Advanced.Histogram(histogramOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData().Histograms.Single().Tags.Should().Equal("tag");

            _context.Advanced.Timer(timerOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData().Timers.Single().Tags.Should().Equal("tag");
        }

        [Fact]
        public void child_with_same_name_are_same_context()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                GroupName = "test"
            };

            var first = _context.Advanced.Counter(counterOptions);
            var second = _context.Advanced.Counter(counterOptions);

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public void data_provider_reflects_child_contexts()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                GroupName = "test",
                MeasurementUnit = Unit.Bytes
            };

            var counter = _context.Advanced.Counter(counterOptions);

            counter.Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData().ChildMetrics.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData().ChildMetrics.Single().Counters.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData().ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(1);

            counter.Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData().ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(2);
        }

        [Fact]
        public void data_provider_reflects_new_metrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "bytes-counter",
                MeasurementUnit = Unit.Bytes,
            };

            _context.Advanced.Counter(counterOptions).Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Single().Name.Should().Be("bytes-counter");
            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Single().Value.Count.Should().Be(1L);
        }

        [Fact]
        public void disabled_child_context_does_not_show_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                GroupName = "test",
                MeasurementUnit = Unit.Bytes
            };

            _context.Advanced.Counter(counterOptions).Increment();

            CurrentData(_context).ChildMetrics.Single()
                .Counters.Single().Name.Should().Be("test");

            _context.Advanced.ShutdownGroup("test");

            CurrentData(_context).ChildMetrics.Should().BeEmpty();
        }

        [Fact]
        public void does_not_throw_on_metrics_of_different_type_with_same_name()
        {
            ((Action)(() =>
            {
                var name = "Test";

                var counterOptions = new CounterOptions
                {
                    Name = name,
                    MeasurementUnit = Unit.Calls,
                };


                var meterOptions = new MeterOptions
                {
                    Name = name,
                    MeasurementUnit = Unit.Calls
                };

                var gaugeOptions = new GaugeOptions
                {
                    Name = name,
                    MeasurementUnit = Unit.Calls
                };

                var histogramOptions = new HistogramOptions
                {
                    Name = name,
                    MeasurementUnit = Unit.Calls
                };

                var timerOptions = new TimerOptions
                {
                    Name = name,
                    MeasurementUnit = Unit.Calls
                };

                _context.Advanced.Gauge(gaugeOptions, () => 0.0);
                _context.Advanced.Counter(counterOptions);
                _context.Advanced.Meter(meterOptions);
                _context.Advanced.Histogram(histogramOptions);
                _context.Advanced.Timer(timerOptions);
            })).ShouldNotThrow();
        }

        [Fact]
        public void metrics_added_are_visible_in_the_data_provider()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Bytes,
            };

            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Should().BeEmpty();
            _context.Advanced.Counter(counterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData().Counters.Should().HaveCount(1);
        }

        [Fact]
        public void metrics_are_present_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "request counter",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _context.Advanced.Counter(counterOptions);

            counter.Increment();

            var counterValue = CurrentData(_context).Counters.Single();

            counterValue.Name.Should().Be("request counter");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);
        }

        [Fact]
        public void MetricsContext_EmptyChildContextIsSameContext()
        {
            //TODO: AH - similar test when only one level of groups is supported
            //var child = _context.Advanced.Group(string.Empty);
            //ReferenceEquals(_context, child).Should().BeTrue();
            //child = _context.Advanced.Group(null);
            //ReferenceEquals(_context, child).Should().BeTrue();
        }

        [Fact]
        public void raises_shutdown_even_on_metrics_disable()
        {
            //TODO: AH - FluentAssertions no longer has MonitorEvents
            //context.MonitorEvents();
            //context.Advanced.CompletelyDisableMetrics();
            //context.ShouldRaise("ContextShuttingDown");
        }

        [Fact]
        public void raises_shutdown_event_on_dispose()
        {
            //TODO: AH - FluentAssertions no longer has MonitorEvents

            //_context.MonitorEvents();
            //_context.Dispose();
            //_context.ShouldRaise("ContextShuttingDown");
        }
    }
}