using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Extensions;
using App.Metrics.Health;
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
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();
        private static readonly IOptions<AppMetricsOptions> Options = Microsoft.Extensions.Options.Options.Create(new AppMetricsOptions());

        private static readonly IHealthCheckManager HealthCheckManager =
            new DefaultHealthCheckManager(LoggerFactory, new HealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options));


        private static readonly IMetricsBuilder MetricsBuilder = new DefaultMetricsBuilder(Options.Value.SystemClock,
            SamplingType.ExponentiallyDecaying);

        private static readonly IMetricsDataManager MetricsDataManager =
            new DefaultMetricsDataManager(LoggerFactory, Options.Value.SystemClock, Enumerable.Empty<EnvironmentInfoEntry>());

        private static readonly Func<IMetricsRegistry> MetricsRegistry = () => new DefaultMetricsRegistry();


        private readonly IMetricsContext _context = new DefaultMetricsContext(Options.Value.GlobalContextName,
            Options.Value.SystemClock, Options.Value.DefaultSamplingType,
            MetricsRegistry, MetricsBuilder, HealthCheckManager, MetricsDataManager);

        public Func<IMetricsContext, MetricsData> CurrentData => ctx => _context.Advanced.MetricsDataManager.GetMetricsData(ctx);

        public Func<IMetricsContext, IMetricsFilter, MetricsData> CurrentDataWithFilter => (ctx, filter) =>
                _context.Advanced.MetricsDataManager.WithFilter(filter).GetMetricsData(ctx);


        [Fact]
        public void can_create_child_context()
        {
            var counterOptions = new CounterOptions
            {
                Name = "counter",
                MeasurementUnit = Unit.Requests,
            };

            _context.Advanced.Group("test").Advanced.Counter(counterOptions);

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

            _context.Advanced.Counter(counterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Single().Tags.Should().Equal("tag");

            _context.Advanced.Meter(meterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Meters.Single().Tags.Should().Equal("tag");

            _context.Advanced.Histogram("test", Unit.None, tags: "tag");
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Histograms.Single().Tags.Should().Equal("tag");

            _context.Advanced.Timer("test", Unit.None, tags: "tag");
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Timers.Single().Tags.Should().Equal("tag");
        }

        [Fact]
        public void child_with_same_name_are_same_context()
        {
            var first = _context.Advanced.Group("test");
            var second = _context.Advanced.Group("test");

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public void data_provider_reflects_child_contexts()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Bytes
            };

            var counter = _context
                .Advanced.Group("test").Advanced
                .Counter(counterOptions);

            counter.Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData(_context).ChildMetrics.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).ChildMetrics.Single().Counters.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(1);

            counter.Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData(_context).ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(2);
        }

        [Fact]
        public void data_provider_reflects_new_metrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Bytes,
            };

            _context.Advanced.Counter(counterOptions).Increment();

            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Should().HaveCount(1);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Single().Name.Should().Be("test");
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Single().Value.Count.Should().Be(1L);
        }

        [Fact]
        public void disabled_child_context_does_not_show_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Bytes
            };

            _context.Advanced.Group("test").Advanced.Counter(counterOptions).Increment();

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

                _context.Advanced.Gauge(gaugeOptions, () => 0.0);
                _context.Advanced.Counter(counterOptions);
                _context.Advanced.Meter(meterOptions);
                _context.Advanced.Histogram(name, Unit.Calls);
                _context.Advanced.Timer(name, Unit.Calls);
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

            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Should().BeEmpty();
            _context.Advanced.Counter(counterOptions);
            _context.Advanced.MetricsDataManager.GetMetricsData(_context).Counters.Should().HaveCount(1);
        }

        [Fact]
        public void metrics_are_present_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _context.Advanced.Counter(counterOptions);

            counter.Increment();

            var counterValue = CurrentData(_context).Counters.Single();

            counterValue.Name.Should().Be("test");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);
        }

        [Fact]
        public void MetricsContext_EmptyChildContextIsSameContext()
        {
            var child = _context.Advanced.Group(string.Empty);
            ReferenceEquals(_context, child).Should().BeTrue();
            child = _context.Advanced.Group(null);
            ReferenceEquals(_context, child).Should().BeTrue();
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