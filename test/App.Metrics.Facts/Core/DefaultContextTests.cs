using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.MetricData;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class DefaultContextTests
    {
        private readonly MetricsContext context = new DefaultMetricsContext();

        public MetricsData CurrentData
        {
            get { return this.context.DataProvider.CurrentMetricsData; }
        }

        [Fact]
        public void MetricsContext_CanCreateSubcontext()
        {
            context.Context("test").Counter("counter", Unit.Requests);

            var counterValue = CurrentData.ChildMetrics.SelectMany(c => c.Counters).Single();

            counterValue.Name.Should().Be("counter");
        }

        [Fact]
        public void MetricsContext_CanPropagateValueTags()
        {
            context.Counter("test", Unit.None, "tag");
            context.DataProvider.CurrentMetricsData.Counters.Single().Tags.Should().Equal(new[] { "tag" });

            context.Meter("test", Unit.None, tags: "tag");
            context.DataProvider.CurrentMetricsData.Meters.Single().Tags.Should().Equal(new[] { "tag" });

            context.Histogram("test", Unit.None, tags: "tag");
            context.DataProvider.CurrentMetricsData.Histograms.Single().Tags.Should().Equal(new[] { "tag" });

            context.Timer("test", Unit.None, tags: "tag");
            context.DataProvider.CurrentMetricsData.Timers.Single().Tags.Should().Equal(new[] { "tag" });
        }

        [Fact]
        public void MetricsContext_ChildWithSameNameAreSameInstance()
        {
            var first = context.Context("test");
            var second = context.Context("test");

            ReferenceEquals(first, second).Should().BeTrue();
        }

        [Fact]
        public void MetricsContext_DataProviderReflectsChildContxts()
        {
            var provider = context.DataProvider;

            var counter = context
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
            var provider = context.DataProvider;

            context.Counter("test", Unit.Bytes).Increment();

            provider.CurrentMetricsData.Counters.Should().HaveCount(1);
            provider.CurrentMetricsData.Counters.Single().Name.Should().Be("test");
            provider.CurrentMetricsData.Counters.Single().Value.Count.Should().Be(1L);
        }

        [Fact]
        public void MetricsContext_DisabledChildContextDoesNotShowInData()
        {
            context.Context("test").Counter("test", Unit.Bytes).Increment();

            CurrentData.ChildMetrics.Single()
                .Counters.Single().Name.Should().Be("test");

            context.ShutdownContext("test");

            CurrentData.ChildMetrics.Should().BeEmpty();
        }

        [Fact]
        public void MetricsContext_DowsNotThrowOnMetricsOfDifferentTypeWithSameName()
        {
            ((Action)(() =>
            {
                var name = "Test";
                context.Gauge(name, () => 0.0, Unit.Calls);
                context.Counter(name, Unit.Calls);
                context.Meter(name, Unit.Calls);
                context.Histogram(name, Unit.Calls);
                context.Timer(name, Unit.Calls);
            })).ShouldNotThrow();
        }

        [Fact]
        public void MetricsContext_EmptyChildContextIsSameContext()
        {
            var child = context.Context(string.Empty);
            ReferenceEquals(context, child).Should().BeTrue();
            child = context.Context(null);
            ReferenceEquals(context, child).Should().BeTrue();
        }

        [Fact]
        public void MetricsContext_MetricsAddedAreVisibleInTheDataProvider()
        {
            context.DataProvider.CurrentMetricsData.Counters.Should().BeEmpty();
            context.Counter("test", Unit.Bytes);
            context.DataProvider.CurrentMetricsData.Counters.Should().HaveCount(1);
        }

        [Fact]
        public void MetricsContext_MetricsArePresentInMetricsData()
        {
            var counter = context.Counter("test", Unit.Requests);

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