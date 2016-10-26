using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.MetricData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;
using System.Threading.Tasks;
using App.Metrics.Registries;

namespace App.Metrics.Facts.Core
{
    public class MetricsContextsTests
    {
        private static readonly IOptions<AppMetricsOptions> Options
           = Microsoft.Extensions.Options.Options.Create(new AppMetricsOptions());

        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        private static readonly IHealthCheckManager HealthCheckManager =
            new DefaultHealthCheckManager(Options, LoggerFactory, new DefaultHealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options));
        private static readonly Func<string, IMetricGroupRegistry> NewMetricsGroupRegistry = name => new DefaultMetricGroupRegistry(name);

        private static readonly IMetricsRegistry Registry = new DefaultMetricsRegistry(LoggerFactory, Options, 
            new EnvironmentInfoBuilder(LoggerFactory), NewMetricsGroupRegistry);

        private static readonly IMetricsDataManager MetricsDataManager = new DefaultMetricsDataManager(Registry);

        private static readonly IMetricsBuilder MetricsBuilder = new DefaultMetricsBuilder(Options.Value.Clock);
      
        private static readonly Func<IMetricsContext, IMetricReporterRegistry> NewReportManager = context => new DefaultMetricReporterRegistry(Options, context, LoggerFactory);

        private readonly IMetricsContext _context = new DefaultMetricsContext(Options, Registry, MetricsBuilder, HealthCheckManager, MetricsDataManager);

        public Func<IMetricsContext, Task<MetricsData>> CurrentData => async ctx => await _context.Advanced.DataManager.GetMetricsDataAsync();

        public Func<IMetricsContext, IMetricsFilter, Task<MetricsData>> CurrentDataWithFilter => async (ctx, filter) => await  _context.Advanced.DataManager.WithFilter(filter).GetMetricsDataAsync();

        public MetricsContextsTests()
        {
            _context.Advanced.ResetMetricsValues();
        }

        [Fact]
        public async Task can_create_child_context()
        {
            var counterOptions = new CounterOptions
            {
                Name = "counter",
                GroupName = "test",
                MeasurementUnit = Unit.Requests,
            };

            _context.Advanced.Counter(counterOptions);

            var data = await CurrentData(_context);

            var counterValue = data.Groups.SelectMany(c => c.Counters).Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public async Task can_filter_metrics_by_type()
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

            var filter = new DefaultMetricsFilter().WhereType(MetricType.Counter);

            counter.Increment();
            meter.Mark(1);

            var currentData = await CurrentDataWithFilter(_context, filter);
            var group = currentData.Groups.Single();

            var counterValue = group.Counters.Single();
            var meterValue = group.Meters.FirstOrDefault();

            counterValue.Name.Should().Be("test");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(meterValue);
        }

        [Fact]
        public async Task can_propergate_value_tags()
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
            _context.Advanced.Meter(meterOptions);
            _context.Advanced.Histogram(histogramOptions);
            _context.Advanced.Timer(timerOptions);

            var data = await CurrentData(_context);
            var group = data.Groups.Single();

            group.Counters.Single().Tags.Should().Equal("tag");
            group.Meters.Single().Tags.Should().Equal("tag");
            group.Histograms.Single().Tags.Should().Equal("tag");
            group.Timers.Single().Tags.Should().Equal("tag");
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
        public async Task data_provider_reflects_child_contexts()
        {
            //TODO: AH - still need this since there are no longer child metrics
            //var counterOptions = new CounterOptions
            //{
            //    Name = "test",
            //    GroupName = "test",
            //    MeasurementUnit = Unit.Bytes
            //};

            //var counter = _context.Advanced.Counter(counterOptions);

            //counter.Increment();

            //var data = await CurrentData(_context);
            //var group = data.Groups.Single();

            //group.ChildMetrics.Should().HaveCount(1);
            //group.ChildMetrics.Single().Counters.Should().HaveCount(1);
            //group.ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(1);

            //counter.Increment();

            //data = await _context.Advanced.DataManager.GetMetricsDataAsync();

            //group = data.Groups.Single();

            //group.ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(2);
        }

        [Fact]
        public async Task data_provider_reflects_new_metrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "bytes-counter",
                MeasurementUnit = Unit.Bytes,
            };

            _context.Advanced.Counter(counterOptions).Increment();

            var data = await CurrentData(_context);
            var group = data.Groups.Single();

            group.Counters.Should().HaveCount(1);
            group.Counters.Single().Name.Should().Be("bytes-counter");
            group.Counters.Single().Value.Count.Should().Be(1L);
        }

        [Fact]
        public async Task disabled_child_context_does_not_show_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                GroupName = "test",
                MeasurementUnit = Unit.Bytes
            };

            _context.Advanced.Counter(counterOptions).Increment();

            var data = await CurrentData(_context);

            data.Groups.Single().Counters.Single().Name.Should().Be("test");

            _context.Advanced.ShutdownGroup("test");

            data = await CurrentData(_context);

            data.Groups.Should().BeEmpty();
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
        public async Task metrics_added_are_visible_in_the_data_provider()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test",
                MeasurementUnit = Unit.Bytes,
            };
            var dataManager = _context.Advanced.DataManager;

            var data = await dataManager.GetMetricsDataAsync();

            data.Groups.Should().BeEmpty();
            _context.Advanced.Counter(counterOptions);

            data = await dataManager.GetMetricsDataAsync();
            data.Groups.Single().Counters.Should().HaveCount(1);
        }

        [Fact]
        public async Task metrics_are_present_in_metrics_data()
        {
            var counterOptions = new CounterOptions
            {
                Name = "request counter",
                MeasurementUnit = Unit.Requests,
            };
            var counter = _context.Advanced.Counter(counterOptions);

            counter.Increment();

            var data = await CurrentData(_context);

            var counterValue = data.Groups.Single().Counters.Single();

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