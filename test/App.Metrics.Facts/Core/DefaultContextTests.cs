using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class DefaultContextTests
    {
        private readonly IMetricsContext _context = new DefaultMetricsContext(Clock.Default);

        public MetricsData CurrentData => _context.DataProvider.CurrentMetricsData;

        [Fact]
        public void MetricsContext_CanCreateSubcontext()
        {
            _context.Context("test").Counter("counter", Unit.Requests);

            var counterValue = CurrentData.ChildMetrics.SelectMany(c => c.Counters).Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public void MetricsContext_CanPropagateValueTags()
        {
            _context.Counter("test", Unit.None, "tag");
            _context.DataProvider.CurrentMetricsData.Counters.Single().Tags.Should().Equal("tag");

            _context.Meter("test", Unit.None, tags: "tag");
            _context.DataProvider.CurrentMetricsData.Meters.Single().Tags.Should().Equal("tag");

            _context.Histogram("test", Unit.None, tags: "tag");
            _context.DataProvider.CurrentMetricsData.Histograms.Single().Tags.Should().Equal("tag");

            _context.Timer("test", Unit.None, tags: "tag");
            _context.DataProvider.CurrentMetricsData.Timers.Single().Tags.Should().Equal("tag");
        }

        [Fact]
        public void MetricsContext_ChildWithSameNameAreSameInstance()
        {
            var first = _context.Context("test");
            var second = _context.Context("test");

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public void MetricsContext_DataProviderReflectsChildContxts()
        {
            var provider = _context.DataProvider;

            var counter = _context
                .Context("test")
                .Counter("test", Unit.Bytes);

            counter.Increment();

            provider.CurrentMetricsData.ChildMetrics.Should().HaveCount(1);
            provider.CurrentMetricsData.ChildMetrics.Single().Counters.Should().HaveCount(1);
            provider.CurrentMetricsData.ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(1);

            counter.Increment();

            provider.CurrentMetricsData.ChildMetrics.Single().Counters.Single().Value.Count.Should().Be(2);
        }

        [Fact]
        public void MetricsContext_DataProviderReflectsNewMetrics()
        {
            var provider = _context.DataProvider;

            _context.Counter("test", Unit.Bytes).Increment();

            provider.CurrentMetricsData.Counters.Should().HaveCount(1);
            provider.CurrentMetricsData.Counters.Single().Name.Should().Be("test");
            provider.CurrentMetricsData.Counters.Single().Value.Count.Should().Be(1L);
        }

        [Fact]
        public void MetricsContext_DisabledChildContextDoesNotShowInData()
        {
            _context.Context("test").Counter("test", Unit.Bytes).Increment();

            CurrentData.ChildMetrics.Single()
                .Counters.Single().Name.Should().Be("test");

            _context.ShutdownContext("test");

            CurrentData.ChildMetrics.Should().BeEmpty();
        }

        [Fact]
        public void MetricsContext_DowsNotThrowOnMetricsOfDifferentTypeWithSameName()
        {
            ((Action)(() =>
            {
                var name = "Test";
                _context.Gauge(name, () => 0.0, Unit.Calls);
                _context.Counter(name, Unit.Calls);
                _context.Meter(name, Unit.Calls);
                _context.Histogram(name, Unit.Calls);
                _context.Timer(name, Unit.Calls);
            })).ShouldNotThrow();
        }

        [Fact]
        public void MetricsContext_EmptyChildContextIsSameContext()
        {
            var child = _context.Context(string.Empty);
            ReferenceEquals(_context, child).Should().BeTrue();
            child = _context.Context(null);
            ReferenceEquals(_context, child).Should().BeTrue();
        }

        [Fact]
        public void MetricsContext_MetricsAddedAreVisibleInTheDataProvider()
        {
            _context.DataProvider.CurrentMetricsData.Counters.Should().BeEmpty();
            _context.Counter("test", Unit.Bytes);
            _context.DataProvider.CurrentMetricsData.Counters.Should().HaveCount(1);
        }

        [Fact]
        public void MetricsContext_MetricsArePresentInMetricsData()
        {
            var counter = _context.Counter("test", Unit.Requests);

            counter.Increment();

            var counterValue = CurrentData.Counters.Single();

            counterValue.Name.Should().Be("test");
            counterValue.Unit.Should().Be(Unit.Requests);
            counterValue.Value.Count.Should().Be(1);
        }

        [Fact]
        public void MetricsContext_RaisesShutdownEventOnDispose()
        {
            //TODO: AH - FluentAssertions no longer has MonitorEvents

            //context.MonitorEvents();
            //context.Dispose();
            //context.ShouldRaise("ContextShuttingDown");
        }

        [Fact]
        public void MetricsContext_RaisesShutdownEventOnMetricsDisable()
        {
            //TODO: AH - FluentAssertions no longer has MonitorEvents
            //context.MonitorEvents();
            //context.Advanced.CompletelyDisableMetrics();
            //context.ShouldRaise("ContextShuttingDown");
        }
    }
}