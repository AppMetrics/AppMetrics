using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Data;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricsFilteringTests : IClassFixture<MetricsWithSamplesFixture>
    {
        private readonly IMetrics _metrics;

        public MetricsFilteringTests(MetricsWithSamplesFixture fixture)
        {
            _metrics = fixture.Metrics;
        }

        [Fact]
        public async Task can_filter_metrics_by_counters()
        {
            var filter = new DefaultMetricsFilter().WhereType(MetricType.Counter);
            var currentData = await _metrics.Advanced.Data.ReadDataAsync(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();
            counterValue.Name.Should().Be("test_counter");
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public async Task can_filter_metrics_by_meters()
        {
            var filter = new DefaultMetricsFilter().WhereType(MetricType.Meter);
            var currentData = await _metrics.Advanced.Data.ReadDataAsync(filter);
            var context = currentData.Contexts.Single();

            var meterValue = context.Meters.Single();
            meterValue.Name.Should().Be("test_meter");
            meterValue.Value.Count.Should().Be(1);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public async Task can_filter_metrics_by_timers()
        {
            var filter = new DefaultMetricsFilter().WhereType(MetricType.Timer);
            var currentData = await _metrics.Advanced.Data.ReadDataAsync(filter);
            var context = currentData.Contexts.Single();

            var timerValue = context.Timers.Single();
            timerValue.Name.Should().Be("test_timer");
            timerValue.Value.TotalTime.Should().Be(10);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }

        [Fact]
        public async Task can_filter_metrics_by_histograms()
        {
            var filter = new DefaultMetricsFilter().WhereType(MetricType.Histogram);
            var currentData = await _metrics.Advanced.Data.ReadDataAsync(filter);
            var context = currentData.Contexts.Single();

            var histogramValue = context.Histograms.Single();
            histogramValue.Name.Should().Be("test_histogram");
            histogramValue.Value.LastValue.Should().Be(5);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }

        [Fact]
        public async Task can_filter_metrics_by_gauge()
        {
            var filter = new DefaultMetricsFilter().WhereType(MetricType.Gauge);
            var currentData = await _metrics.Advanced.Data.ReadDataAsync(filter);
            var context = currentData.Contexts.Single();

            var gaugeValue = context.Gauges.Single();
            gaugeValue.Name.Should().Be("test_gauge");
            gaugeValue.Value.Should().Be(8);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }
    }
}